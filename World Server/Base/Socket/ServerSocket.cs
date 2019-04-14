using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorldServer.Base
{
    public class ServerSocket
    {
        public event Action<ClientWrapper> OnClientConnect, OnClientDisconnect;
        public event Action<byte[], int, ClientWrapper> OnClientReceive;

        private Dictionary<int, int> BruteforceProtection;
        private const int TimeLimit = 1000 * 15; // 1 connection every 10 seconds for one ip
        private object SyncRoot;

        private Socket Connection;
        private ushort port;
        private bool enabled;
        private Thread thread;
        public ServerSocket()
        {
            this.Connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.SyncRoot = new object();
            thread = new Thread(doSyncAccept);
            thread.Start();
        }      
        private void Accept()
        {
            Connection.BeginAccept(new AsyncCallback(Accept_Callback), null);
        }
        private void Accept_Callback(IAsyncResult asyncResult)
        {
            try
            {
                Socket accepted = Connection.EndAccept(asyncResult);
                if (accepted.Connected)
                {
                    string ip = (accepted.RemoteEndPoint as IPEndPoint).Address.ToString();
                    string Localip = (accepted.LocalEndPoint as IPEndPoint).Address.ToString();
                    int ipHash = ip.GetHashCode();
                    ClientWrapper wrapper = new ClientWrapper();
                    wrapper.Create(accepted, this, OnClientReceive);
                    wrapper.Alive = true;
                    wrapper.IP = ip;
                    wrapper.LocalIp = Localip;
                    if (this.OnClientConnect != null) this.OnClientConnect(wrapper);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            Accept();
        }


        public void stoptheard()
        {
            thread = null;
        }
        public void Enable(ushort port)
        {
            this.port = port;
            this.Connection.Bind(new IPEndPoint(IPAddress.Any, this.port));
            this.Connection.Listen(100);
            this.enabled = true;
            BruteforceProtection = new Dictionary<int, int>();
        }

        public bool PrintoutIPs = false;
        private void doSyncAccept()
        {
            while (true)
            {
                if (this.enabled)
                {
                    try
                    {
                        processSocket(this.Connection.Accept());
                    }
                    catch (Exception es) { Console.WriteLine(es); }
                }
                Thread.Sleep(1);
            }
        }

        private void processSocket(Socket socket)
        {
            try
            {
                string ip = (socket.RemoteEndPoint as IPEndPoint).Address.ToString();
                string Localip = (socket.LocalEndPoint as IPEndPoint).Address.ToString();
                int ipHash = ip.GetHashCode();

                int time = Time32.Now.GetHashCode();
                int oldValue;
                if (!BruteforceProtection.TryGetValue(ipHash, out oldValue))
                {
                    BruteforceProtection[ipHash] = time;
                }
                else
                {
                    if (time - oldValue < TimeLimit)
                    {
                        if (PrintoutIPs) Console.WriteLine("Dropped connection: " + ip);
                        socket.Disconnect(false);
                        socket.Close();
                        return;
                    }
                    else
                    {
                        BruteforceProtection[ipHash] = time;
                        if (PrintoutIPs) Console.WriteLine("Allowed connection: " + ip);
                    }
                }

                ClientWrapper wrapper = new ClientWrapper();
                wrapper.Create(socket, this, OnClientReceive);
                wrapper.Alive = true;
                wrapper.IP = ip;
                wrapper.LocalIp = Localip;
                if (this.OnClientConnect != null) this.OnClientConnect(wrapper);
            }
            catch (Exception es) { Console.WriteLine(es); }
        }

        public void Reset()
        {
            this.Disable();
            this.Enable();
        }

        public void Disable()
        {
            this.enabled = false;
            this.Connection.Close(1);
        }

        public void Enable()
        {
            if (!this.enabled)
            {
                Connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.Connection.Bind(new IPEndPoint(IPAddress.Any, this.port));
                this.Connection.Listen(100);
                this.enabled = true;
            }
        }

        public void InvokeDisconnect(ClientWrapper Client)
        {
            if (this.OnClientDisconnect != null)
                this.OnClientDisconnect(Client);
        }

        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
        }
    }
}
