using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldServer.Base;
using WorldServer.Tables;

namespace WorldServer
{
    class Program
    {
        public static Dictionary<int, Server> GameServersPool = new Dictionary<int, Server>();
        static void Main(string[] args)
        {
            Console.Title = "Konquer - World";

            Base.Attributes.Load();

            // Database
            Console.WriteLine("Loading servers table.");
            ServerTable.LoadInfo();// From the auth DB 
            foreach (var svr in ServerTable.ServersTable.Values)
                CreateServer(svr);
            Console.WriteLine($"Total {GameServersPool.Count} GameServers are online!");
            Console.WriteLine();
            while (true)
                HandleCommands();
        }

        private static void CreateServer(BaseServer server)
        {
            if (GameServersPool.ContainsKey(server.ID))
            {
                Console.WriteLine($"Server {server.Name} already exists!");
                return;
            }
            var svr = new Server((ushort)server.Port, server.Name);
            GameServersPool.Add(server.ID, svr);
        }

        static void HandleCommands()
        {
            string cmd = Console.ReadLine();
            string[] cmds = cmd.Split(' ');
            switch (cmds[0])
            {
                case "clear":
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
    }
}
