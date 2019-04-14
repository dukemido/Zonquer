using Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldServer.Base;
using WorldServer.Game;
using WorldServer.MapObjects;
using WorldServer.Tables;

namespace WorldServer.Packets
{
    public class CMsgRegister
    {
        string Name;
        ushort Body;
        byte Class;
        public void Deserialize(byte[] Buffer)
        {
            Name = Encoding.Default.GetString(Buffer, 24, 16).Replace("\0", "");
            Body = BitConverter.ToUInt16(Buffer, 72);
            Class = Buffer[74];
        }
        [PacketMethod(1001)]
        public static void HandleCreate(byte[] Buffer, GameClient client)
        {
            if (client.Action == 1)
            {
                var CE = new CMsgRegister();
                CE.Deserialize(Buffer);
                foreach (char p in CE.Name)
                    if (Constants.InvalidCharacters.Contains(p))
                    {
                        client.Send(new CMsgTalk(Color.Orange, ChatType.PopUP, "Invalid Characters in the name!", "", "ALLUSERS"));
                        return;
                    }
                // TODO : Check if the name exists in the database.
                if (PlayerTable.CheckName(CE.Name, client.MyServer))
                {
                    client.Send(new CMsgTalk(Color.Orange, ChatType.PopUP, "Choose another name!", "", "ALLUSERS"));
                    return;
                }
                // IF DONE
                client.Player = new Player();
                client.Player.UID = client.CreatedUID;
                client.Player.Name = CE.Name;
                client.Player.Level = 1;
                client.Player.Spouse = "";
                client.Player.Class = GetClass(CE.Class);
                client.Player.Body = CE.Body;
                client.Player.Face = 150;
                client.Player.Hairstyle = 537;
                client.Player.Mesh = (uint)((10000 * client.Player.Face) + client.Player.Body);
                PlayerTable.Insert(client.Player);
                client.Send(new CMsgTalk(Color.Orange, ChatType.PopUP, "ANSWER_OK", "", "ALLUSERS"));
                client.Send(new CMsgUserInfo(client.Player));
                Console.WriteLine($"[{client.IP}] {client.Player.Name} has been created!");
            }
        }
        public static byte GetClass(byte ec)
        {
            byte Class = 0;
            switch (ec)
            {
                case 0:
                case 1: Class = 100; break;
                case 2:
                case 3: Class = 11; break;
                case 4:
                case 5: Class = 41; break;
                case 6:
                case 7: Class = 21; break;
                case 8:
                case 9: Class = 51; break;
                case 10:
                case 11: Class = 61; break;
                case 12:
                case 13: Class = 71; break;
                case 14:
                case 15: Class = 81; break;
                case 16:// 16 & 17 : Chaser
                case 17:
                    {
                        Class = 161;
                        //windwalker = (byte)WindWalker.CharacterType.Chaser;
                        break;
                    }
                case 18://18 & 19 : Stomper
                case 19:
                    {
                        Class = 161;
                        //windwalker = (byte)WindWalker.CharacterType.Stomper;
                        break;
                    }
            }
            return Class;
        }
    }
}
