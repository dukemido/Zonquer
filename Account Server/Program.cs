using AccountServer.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccountServer.Tables;
using AccountServer.Packets;
using AccountServer.Sockets;
using System.Threading;
using KonquerCore;
using MySql.Data.MySqlClient;

namespace AccountServer
{
    class Program
    {
        public static string connectionString { get; private set; }
        public static Random Random = new Random();
        public static DateTime StartDate;
        public static Socks AuthServer;
        static void Main(string[] args)
        {
            StartDate = DateTime.Now;
            Console.Title = "Konquer - AuthServer";

            var cfgFile = new IniFile("Config.ini");
            int Port = cfgFile.ReadInt32("Server", "AuthPort", 0);
            string host = cfgFile.ReadString("MySql", "Host"),
                username = cfgFile.ReadString("MySql", "Username"),
                password = cfgFile.ReadString("MySql", "Password"),
                database = cfgFile.ReadString("MySql", "Database");

            var stringBuilder = new MySqlConnectionStringBuilder();
            stringBuilder.Server = host;
            stringBuilder.UserID = username;
            stringBuilder.Password = password;
            stringBuilder.Database = database;
            connectionString = stringBuilder.GetConnectionString(true);

            using (var testConn = new MySqlConnection(connectionString))
            {
                try
                {
                    testConn.Open();
                    Console.WriteLine("Valid connection string.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[DBError] {e.Message}.");
                    Console.ReadLine();
                    return;
                }
            }

            Servers.LoadInfo();
            AuthCryptography.PrepareAuthCryptography();
            AuthServer = new Socks(Port, 0);

            while (true)
                HandleCommands();
        }
        static void HandleCommands()
        {
            string[] cmds = Console.ReadLine().Split(' ');
            try
            {
                switch (cmds[0])
                {
                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
        }
    }
}
