using Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldServer;

namespace WorldServer.Base
{
    public class Server
    {
        public string connectionString
        {
            get;
            private set;
        }
        public string serverName
        {
            get;
            private set;// can't be set except from here.
        }

        public ConcurrentDictionary<uint, GameClient> ConfirmationPool = new ConcurrentDictionary<uint, GameClient>();
        public ConcurrentDictionary<uint, GameClient> GamePool = new ConcurrentDictionary<uint, Client.GameClient>();
        public Server(ushort GamePort, string serverName)
        {
            this.serverName = serverName;
            connectionString = $"Data Source=DESKTOP-44PC44A\\SQLEXPRESS;Database=Zonquer_{serverName};Integrated Security= true";

            //socket
            var GameServer = new ServerSocket();
            GameServer.OnClientConnect += GameServer_OnClientConnect;
            GameServer.OnClientReceive += GameServer_OnClientReceive;
            GameServer.OnClientDisconnect += GameServer_OnClientDisconnect;
            GameServer.Enable(GamePort);
            Console.WriteLine($"[{serverName}] is enabled on {GamePort}");
        }
        #region Socket Events
        void GameServer_OnClientConnect(ClientWrapper obj)
        {
            Client.GameClient client = new Client.GameClient(obj);
            client.Send(client.DHKeyExchange.CreateServerKeyPacket());
            obj.Connector = client;
            //ref to my server 
            client.MyServer = this;
        }
        void GameServer_OnClientDisconnect(ClientWrapper obj)
        {
            if (obj.Connector != null)
                (obj.Connector as Client.GameClient).Disconnect();
            else
                obj.Disconnect();
        }
        void GameServer_OnClientReceive(byte[] buffer, int length, ClientWrapper obj)
        {
            if (obj.Connector == null)
            {
                obj.Disconnect();
                return;
            }
            Client.GameClient Client = obj.Connector as Client.GameClient;
            if (Client.Exchange)
            {
                Client.Exchange = false;
                Client.Action = 1;
                var crypto = new Cryptography.GameCryptography(System.Text.Encoding.Default.GetBytes(Constants.GameCryptographyKey));
                byte[] otherData = new byte[length];
                Array.Copy(buffer, otherData, length);
                crypto.Decrypt(otherData, length);

                bool extra = false;
                int pos = 0;
                if (BitConverter.ToInt32(otherData, length - 140) == 128)//no extra packet
                {
                    pos = length - 140;
                    Client.Cryptography.Decrypt(buffer, length);
                }
                else if (BitConverter.ToInt32(otherData, length - 176) == 128)//extra packet
                {
                    pos = length - 176;
                    extra = true;
                    Client.Cryptography.Decrypt(buffer, length - 36);
                }
                int len = BitConverter.ToInt32(buffer, pos); pos += 4;
                if (len != 128)
                {
                    Client.Disconnect();
                    return;
                }
                byte[] pubKey = new byte[128];
                for (int x = 0; x < len; x++, pos++) pubKey[x] = buffer[pos];

                string PubKey = System.Text.Encoding.Default.GetString(pubKey);
                Client.Cryptography = Client.DHKeyExchange.HandleClientKeyPacket(PubKey, Client.Cryptography);

                if (extra)
                {
                    byte[] data = new byte[36];
                    Buffer.BlockCopy(buffer, length - 36, data, 0, 36);
                    processData(data, 36, Client);
                }
            }
            else
            {
                processData(buffer, length, Client);
            }
        }
        void processData(byte[] buffer, int length, Client.GameClient Client)
        {
            Client.Cryptography.Decrypt(buffer, length);
            Client.Queue.Enqueue(buffer, length);
            if (Client.Queue.CurrentLength > 1224)
            {
                Console.WriteLine("[Disconnect]Reason:The packet size is too big. " + Client.Queue.CurrentLength);
                Client.Disconnect();
                return;
            }
            while (Client.Queue.CanDequeue())
            {
                byte[] data = Client.Queue.Dequeue();

                //startHandling
                Packets.Handler.HandlePacket(data, Client);
            }
        }
        #endregion
    }
}
