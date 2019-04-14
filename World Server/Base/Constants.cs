using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldServer.Base
{
    public class Constants
    {

        public static string ServerKey = "TQServer",
            ClientKey = "TQClient",
            GameCryptographyKey = "xztufadwedfggfue",
            AuthconnectionString = "Server=localhost;username=root;password=123456;database=auth;",
            connectionString = "Data Source=DESKTOP-44PC44A\\SQLEXPRESS;Database=Zonquer_World;Integrated Security= true";
        public static List<char> InvalidCharacters = new List<char>() { ' ', '[', '{', '}', '(', ')', ']', '#', '*', '\\', '/', '<', '>', '"', '|', '=', '' };
    }
}
