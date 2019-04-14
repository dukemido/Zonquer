using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;

namespace AccountServer.Tables
{
    public enum AccountRole : byte
    {
        Player = 0,
        Banned = 1,
        PM = 2
    }
    public class Accounts
    {
        public string Username = "",
            Password = "",
            IPAddress = "";
        private string UidsList = "";
        public bool Found = false;
        /// <summary>
        /// int : ServerID is my key
        /// uint : UID in that server
        /// </summary>
        public Dictionary<int, uint> UIDS;
        public AccountRole Role;
        public Accounts(string username)
        {
            UIDS = new Dictionary<int, uint>();
            using (var conn = new MySqlConnection(Program.connectionString))
            using (var cmd = new MySqlCommand($"SELECT * FROM accounts WHERE Username='{username}'", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Username = reader.GetString("Username");
                        Password = reader.GetString("Password");
                        IPAddress = reader.GetString("IPAddress");
                        Role = (AccountRole)Convert.ToByte(reader["Role"]);
                        UidsList = reader.GetString("UidsList");
                        // ServerID#UID=Server2ID#UID
                        // Example : 1#1020032=32#1002390
                        if (!string.IsNullOrEmpty(UidsList))
                        {
                            string[] Accs = UidsList.Split('=');
                            for (int i = 0; i < Accs.Length; i++)
                            {
                                string[] data = Accs[i].Split('#');
                                UIDS.Add(int.Parse(data[0]), uint.Parse(data[1]));
                            }
                            Console.WriteLine();
                            Console.WriteLine($"Account info for user : {Username}");
                            foreach (var item in UIDS)
                                Console.WriteLine($"- UID {item.Value} on server {Servers.ServersTable.Where(e => e.Value.ID == item.Key).SingleOrDefault().Key}");
                            Console.WriteLine();
                        }
                        Found = true;
                    }
                }
            }

        }

        public void FinalizeLogin()
        {
            string finalList = "";
            bool s = true;
            foreach (var item in UIDS)
            {
                if (!s)
                    finalList += "=";
                finalList += $"{item.Key}#{item.Value}";
                s = false;
            }
            using (var conn = new MySqlConnection(Program.connectionString))
            using (var cmd = new MySqlCommand($"UPDATE accounts SET IPAddress='{IPAddress}', UidsList='{finalList}'", conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

}
