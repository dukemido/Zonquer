using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using WorldServer.Base;

namespace WorldServer.Tables
{
    public class BaseServer
    {
        public string Name;
        public int ID;
        public int Port;
        public string IP;
    }
    public class ServerTable
    {
        public static Dictionary<string, BaseServer> ServersTable;


        public static void LoadInfo()
        {
            try
            {
                ServersTable = new Dictionary<string, BaseServer>();
                using (var conn = new MySqlConnection(Constants.AuthconnectionString))
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
                        }
                    }
                }
            }
            catch (Exception e)
            { Console.WriteLine(e.ToString()); }
            Console.WriteLine();
        }
    }
}
