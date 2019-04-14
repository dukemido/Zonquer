using Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldServer.Base;

namespace WorldServer.Packets
{
    public enum ChatType : uint
    {
        Talk = 2000,
        Whisper = 2001,
        Team = 2003,
        Guild = 2004,
        Clan = 2006,
        System = 2007,
        Friend = 2009,
        Center = 2011,
        TopLeft = 2012,
        Service = 2014,
        Tip = 2015,
        Unknown = 2016,
        World = 2021,
        Qualifier = 2022,
        ChiSystem = 2024,
        Ally = 2025,
        JiangHu = 2026,
        InnerPower = 2027,
        PopUP = 2100,
        Dialog = 2101,
        HawkMessage = 2104,
        Website = 2105,
        FirstRightCorner = 2108,
        ContinueRightCorner = 2109,
        SystemWhisper = 2110,
        GuildAnnouncement = 2111,
        Agate = 2115,
        Middle = 2115,
        CrossServer = 2402,
        BroadcastMessage = 2500,
        Monster = 2600,
        SlideFromRight = 100000,
        SlideFromRightRedVib = 1000000,
        WhiteVibrate = 10000000
    }
    public class CMsgTalk : BasePacket
    {
        #region Properties
        byte[] Buffer;
        string Sender, Recepient, Message;
        Color Color;
        ChatType Type;
        uint Mesh;

        public CMsgTalk(Color Color, ChatType Type = ChatType.Talk, string Message = "", string Sender = "", string Recepient = "", uint Mesh = 0)
        {
            this.Sender = Sender;
            this.Recepient = Recepient;
            this.Message = Message;
            this.Color = Color;
            this.Type = Type;
            this.Mesh = Mesh;
            Buffer = new byte[(((32 + Sender.Length) + Recepient.Length) + Message.Length) + 18];
            Writer.WriteUInt16((ushort)(Buffer.Length - 8), 0, Buffer);
            Writer.WriteUInt16(1004, 2, Buffer);
        }
        public override byte[] ToArray()
        {
            Writer.WriteUInt32((uint)Color.ToArgb(), 4, Buffer);
            Writer.WriteUInt32((uint)Type, 8, Buffer);
            /*Writer.WriteUInt32(MessageUID1, 16, Buffer);
            Writer.WriteUInt32(MessageUID2, 20, Buffer);
            */
            Writer.WriteUInt32(Mesh, 20, Buffer);
            Writer.WriteStringList(new System.Collections.Generic.List<string>() { Sender, Recepient, "", Message }, 22, Buffer);
            return Buffer;
        }

        #endregion
        [PacketMethod(1004)]
        public static void HandleMessages(byte[] packet, GameClient client)
        {

        }
    }
}
