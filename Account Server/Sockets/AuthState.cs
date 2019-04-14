using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccountServer.Packets;
using AccountServer.Cryptography;
using System.Net.Sockets;
using AccountServer.Tables;

namespace AccountServer.Sockets
{
    public class AuthState
    {
        #region Socket Fields
        private bool Alive;
        private byte[] buffer;
        private Socket Socket;
        #endregion

        private AuthCryptography Cryptographer;
        public AccountStructure Account;


        public AuthState(Socket socket)
        {
            // Socket objects
            this.Socket = socket;
            this.Alive = true;
            buffer = new byte[2048];

            Cryptographer = new AuthCryptography();
        }

        #region Socket Methods
        public void Send(byte[] buffer)
        {
            if (Alive)
            {
                byte[] _buffer = new byte[buffer.Length];
                Buffer.BlockCopy(buffer, 0, _buffer, 0, buffer.Length);
                lock (Cryptographer)
                {
                    Cryptographer.Encrypt(_buffer);
                    try
                    {
                        Socket.Send(_buffer);
                    }
                    catch (Exception e)//posible proxy/food with proxy
                    {
                        System.Console.WriteLine(e.ToString());
                        this.Disconnect();
                    }
                }
            }
        }
        public void Disconnect(string reason = "")
        {
            if (Alive)
            {
                this.Alive = false;
                this.Socket.Shutdown(SocketShutdown.Both);// Safe disconnection.

                if (reason != "")
                    Console.WriteLine($"Disconnection on Socket -> {reason}");
                else
                    Console.WriteLine("Disconnection -> Normal");
            }
        }
        internal void Receive()
        {
            try
            {
                Socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, socketReceive, Socket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Disconnect();
            }
        }

        private void socketReceive(IAsyncResult ar)
        {
            try
            {
                int receivedLength = Socket.EndReceive(ar);
                if (receivedLength > 0)
                {
                    byte[] received = new byte[receivedLength];
                    Array.Copy(buffer, received, receivedLength);
                    Cryptographer.Decrypt(received);
                    HandlePacket(received);
                }
                else
                    Disconnect("Length received is 0");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void HandlePacket(byte[] received)
        {
            ushort packetLength = BitConverter.ToUInt16(received, 0),
                packetId = BitConverter.ToUInt16(received, 2);

            switch (packetId)
            {
                case 1086:
                    {
                        string IP = (Socket?.RemoteEndPoint as System.Net.IPEndPoint).Address.ToString();
                        Account = MsgAccountSRP6Ex.Deserialize(received);
                        var tableInfo = new Tables.Accounts(Account.Username);
                        if (!tableInfo.Found)
                        {
                            Send(MsgConnectEx.Rejected(MsgConnectEx.RejectionCode.InvalidInfo));
                            return;
                        }
                        string enc = PasswordCryptography.EncryptPassword(tableInfo.Password);
                        if (Account.Password != enc)
                        {
                            Send(MsgConnectEx.Rejected(MsgConnectEx.RejectionCode.InvalidInfo));
                            Console.WriteLine($"[{IP}] {Account.Username} --> INVALID ON [{Account.Server}].");
                            return;
                        }
                        if (!Servers.ServersTable.ContainsKey(Account.Server))
                        {
                            Console.WriteLine($"[{IP}] {Account.Username} --> [{Account.Server}] INVALID SERVER.");
                            Send(MsgConnectEx.Rejected(MsgConnectEx.RejectionCode.ServersNotConfigured));
                            return;
                        }
                        var server = Servers.ServersTable.Where(e => e.Key == Account.Server).SingleOrDefault().Value;
                        if (!tableInfo.UIDS.ContainsKey(server.ID))
                        {
                            uint lastuid = 0;
                            while (lastuid == 0)
                                lastuid = (uint)Config.GetLastUID();
                            tableInfo.UIDS.Add(server.ID, lastuid);// Gets the next UID
                            Console.WriteLine($"[UID] [{lastuid}] created for user : {Account.Username} .");
                        }
                        Send(
                            MsgConnectEx.Verified(tableInfo.UIDS.Where(e => e.Key == server.ID).SingleOrDefault().Value,
                            server.IP, server.Port));
                        tableInfo.IPAddress = IP;
                        tableInfo.FinalizeLogin();// Save info to the database.
                        Console.WriteLine($"[{IP}] {Account.Username} is transfered to {Account.Server} status : {tableInfo.Role.ToString()}.");
                        break;
                    }
                default:
                    {
                        Console.WriteLine($"Unknown packet-> Id: {packetId} Length: {packetLength}.");
                        break;
                    }
            }
        }
        #endregion
    }
}
