using System;
using WorldServer.Base;
using WorldServer.Base.Cryptography;
using WorldServer.MapObjects;
using WorldServer.Packets;

namespace Client
{
    public class GameClient
    {
        public bool IsCreating;
        public ConcurrentPacketQueue Queue;
        public PacketFilter PacketFilter;
        public bool Exchange = true, Attackable;
        public byte Action = 0;
        private ClientWrapper _socket;
        public DHKeyExchange.ServerKeyExchange DHKeyExchange;
        public GameCryptography Cryptography;
        public Server MyServer;
        public Player Player;
        public uint CreatedUID;
        public string IP
        {
            get { return _socket.IP; }
        }
        public GameClient(ClientWrapper socket)
        {
            Queue = new ConcurrentPacketQueue();
            PacketFilter = new PacketFilter() { { 10010, 10 }, { 10005, 7 }, { 2064, 4 }, { 2032, 3 }, { 1027, 2 } };
            Attackable = false;
            Action = 0;
            _socket = socket;
            Cryptography = new GameCryptography(System.Text.Encoding.Default.GetBytes(Constants.GameCryptographyKey));
            DHKeyExchange = new DHKeyExchange.ServerKeyExchange();
        }
        #region Socket Helper Method
        public void Send(byte[] buffer, bool SendFromCroos = false)
        {
            if (!_socket.Alive) return;
            ushort length = BitConverter.ToUInt16(buffer, 0);
            ushort id = BitConverter.ToUInt16(buffer, 2);
            if (id == 10014)
            { }
            if (length >= 1024 && buffer.Length > length)
            {
                return;
            }
            byte[] _buffer = new byte[buffer.Length];
            if (length == 0)
                Writer.WriteUInt16((ushort)(buffer.Length - 8), 0, buffer);
            Buffer.BlockCopy(buffer, 0, _buffer, 0, buffer.Length);
            Writer.WriteString(Constants.ServerKey, _buffer.Length - 8, _buffer);
            try
            {

                lock (_socket)
                {
                    if (!_socket.Alive) return;
                    lock (Cryptography)
                    {
                        Cryptography.Encrypt(_buffer, _buffer.Length);
                        _socket.Send(_buffer);
                    }
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                _socket.Alive = false;
                Disconnect();
            }
        }
        public void Disconnect()
        {
            if (_socket.Connector != null)
            {
                _socket.Disconnect();
                //saveDatabase 
            }
            else
            {
                //saveDatabase 
            }
        }
        #endregion


        internal void Send(BasePacket Packet)
        {
            this.Send(Packet.ToArray());
        }
    }
}