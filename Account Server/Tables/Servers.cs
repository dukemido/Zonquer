using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;

namespace AccountServer.Tables
{
    public class BaseServer
    {
        public string Name;
        public int ID;
        public int Port;
        public string IP;
    }
    public class Servers
    {
        public static Dictionary<string, BaseServer> ServersTable;
        public static void LoadInfo()
        {
            try
            {
                ServersTable = new Dictionary<string, BaseServer>();
                using (var conn = new MySqlConnection(Program.connectionString))
                using (var cmd = new MySqlCommand("SELECT * FROM world_servers", conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var baseServer = new BaseServer();
                            baseServer.ID = Convert.ToInt32(reader["ID"]);
                            baseServer.Port = Convert.ToInt32(reader["Port"]);
                            baseServer.Name = reader["Name"].ToString();
                            baseServer.IP = reader["IP"].ToString();
                            if (ServersTable.ContainsKey(baseServer.Name))
                            {
                                Console.WriteLine($"Error : Server {baseServer.Name} already exists!");
                                continue;
                            }
                            ServersTable.Add(baseServer.Name, baseServer);
                            Console.WriteLine($"[{baseServer.Name}] on port : {baseServer.Port}");
                        }
                        Console.WriteLine($"Loaded {ServersTable.Count} servers in the database!");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine();
        }
    }
}
