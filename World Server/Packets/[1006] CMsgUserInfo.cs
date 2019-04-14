using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldServer.MapObjects;

namespace WorldServer.Packets
{
    public class CMsgUserInfo : BasePacket
    {
        byte[] Buffer;
        public CMsgUserInfo(Player playerObj)
        {
            var memstream = new MemoryStream();
            var writer = new BinaryWriter(memstream);
            writer.Write((ushort)0);
            writer.Write((ushort)1006);//2
            writer.Write((uint)0);//4
            writer.Write(playerObj.UID);//8
            writer.Write((ushort)0);//12
            writer.Write(playerObj.Mesh);//14
            writer.Write(playerObj.Hairstyle);//18
            writer.Write(playerObj.Silvers);//20
            writer.Write(playerObj.ConquerPoints);//28
            writer.Write(playerObj.Experience);//32
            for (int i = 0; i < 2; i++)//40 48
                writer.Write((ulong)0);
            writer.Write((uint)0);//56
            writer.Write(playerObj.Strength);//60
            writer.Write(playerObj.Agility);//62
            writer.Write(playerObj.Vitality);//64
            writer.Write(playerObj.Spirit);//66
            writer.Write(playerObj.Attributes);//68
            writer.Write((ushort)0);//70
            writer.Write(playerObj.Mana);//72
            writer.Write(playerObj.Hitpoints);//74
            writer.Write(playerObj.PKPoints);//76
            writer.Write(playerObj.Level);//78
            writer.Write(playerObj.Class);//79
            writer.Write(playerObj.FirstRebornClass);//80
            writer.Write(playerObj.SecondRebornClass);//81
            writer.Write((byte)0);//82
            writer.Write(playerObj.Reborn);//83
            writer.Write((byte)0);//84
            writer.Write(playerObj.QuizPoints);//85
            writer.Write(playerObj.WindWalker);//89
            writer.Write(playerObj.EnlightenPoints);//93
            writer.Write((ushort)playerObj.VIPLevel);//94
            for (int i = 0; i < 7; i++)//
                writer.Write((uint)0);// 118
            writer.Write(playerObj.Country);//124
            writer.Write((ulong)0);//126
            writer.Write((uint)0);//134
            writer.Write((byte)3);//138
            writer.Write(playerObj.Name);//140
            writer.Write(playerObj.Spouse);
            int packetlength = (int)memstream.Length;
            memstream.Position = 0;
            writer.Write((ushort)packetlength);
            memstream.Position = memstream.Length;
            writer.Write(Encoding.Default.GetBytes("TQServer"));
            memstream.Position = 0;
            Buffer = new byte[memstream.Length];
            memstream.Read(Buffer, 0, Buffer.Length);
            writer.Close();
            memstream.Close();
        }
        public override byte[] ToArray()
        {
            return Buffer;
        }
    }
}
