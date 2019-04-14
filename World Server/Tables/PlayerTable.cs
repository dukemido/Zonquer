using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldServer.Base;
using WorldServer.MapObjects;

namespace WorldServer.Tables
{
    public class PlayerTable
    {
        public static bool CheckName(string Name, Server server)
        {
            try
            {
                bool Found = false;
                using (var conn = new MySqlConnection(server.connectionString))
                using (var cmd = new MySqlCommand($"SELECT * FROM players WHERE Name='{Name}'", conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                        if (reader.Read())
                            Found = true;
                }
                return Found;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
        public static bool Load(uint UID, ref Player Player, Server server)
        {
            try
            {
                bool Found = false;
                using (var conn = new MySqlConnection(server.connectionString))
                using (var cmd = new MySqlCommand($"SELECT * FROM players WHERE EntityUID='{UID}'", conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Found = true;
                            Player = new Player();
                            Player.Name = reader["Name"].ToString();
                            Player.Spouse = reader["Spouse"].ToString();
                            Player.UID = Convert.ToUInt32(reader["EntityUID"]);
                            Player.Level = Convert.ToByte(reader["Level"]);
                            Player.Class = Convert.ToByte(reader["Class"]);
                            Player.Country = Convert.ToUInt16(reader["Country"]);
                            Player.Body = Convert.ToUInt16(reader["Body"]);
                            Player.Face = Convert.ToUInt16(reader["Face"]);
                            Player.Hairstyle = Convert.ToUInt16(reader["Hair"]);
                            Player.Mesh = (uint)((10000 * Player.Face) + Player.Body);
                        }
                    }
                }
                return Found;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
        public static void Insert(Player Player)
        {
            try
            {
                using (var conn = new MySqlConnection(Constants.connectionString))
                using (var cmd = new MySqlCommand($"INSERT INTO players (Name,EntityUID,Level,Class,Spouse,Hair,Face,Body,Country) Values " +
                    $"('{Player.Name}','{Player.UID}','{Player.Level}','{Player.Class}','{Player.Spouse}','{Player.Hairstyle}','{Player.Face}',"
                    + $"'{Player.Body}','{Player.Country}')", conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}
