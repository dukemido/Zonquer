using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace AccountServer.Sockets
{
    public class Socks
    {
        int port, backLog;
        Socket mainServer;
        public Socks(int port, int backLog = 0)
        {
            try
            {
                this.port = port;

                mainServer = new Socket(AddressFamily.InterNetwork
                    , SocketType.Stream, ProtocolType.Tcp);
                mainServer.NoDelay = true;
                mainServer.Bind(new IPEndPoint(IPAddress.Any, port));// Accept Any IP
                mainServer.Listen(backLog); // 0 for no limit.
                Console.WriteLine($"Server enabled on port {port}");
                mainServer.BeginAccept(new AsyncCallback(BeginAccept), mainServer);
            }
            catch
            {
                Console.WriteLine("Exception on Socket.");
            }
        }

        private void BeginAccept(IAsyncResult ar)
        {
            try
            {
                Socket listener = mainServer.EndAccept(ar);
                var sock = new AuthState(listener);
                sock.Receive();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                mainServer.BeginAccept(new AsyncCallback(BeginAccept), mainServer);
            }
        }
    }
}
