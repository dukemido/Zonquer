using Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldServer.Base;
using WorldServer.MapObjects;

namespace WorldServer.Packets
{
    public class CMsgAction : BasePacket
    {
        public enum ActionType : uint
        {
            SetLocation = 74,
            SwitchPK = 96,
            SetSurroundings = 114
        }
        #region Properties
        byte[] Buffer;
        public uint UID
        {
            get { return BitConverter.ToUInt32(Buffer, 8); }
            set { Writer.WriteUInt32(value, 8, Buffer); }
        }
        public uint dwParam
        {
            get { return BitConverter.ToUInt32(Buffer, 12); }
            set { Writer.WriteUInt32(value, 12, Buffer); }
        }
        public ActionType SubType
        {
            get { return (ActionType)BitConverter.ToUInt16(Buffer, 24); }
            set { Writer.WriteUInt32((uint)value, 24, Buffer); }
        }
        public ushort X
        {
            get { return BitConverter.ToUInt16(Buffer, 28); }
            set { Writer.WriteUInt16(value, 28, Buffer); }
        }
        public ushort Y
        {
            get { return BitConverter.ToUInt16(Buffer, 30); }
            set { Writer.WriteUInt16(value, 30, Buffer); }
        }
        public void Deserialize(byte[] Buffer)
        {
            this.Buffer = Buffer;
        }
        public override byte[] ToArray()
        {
            return this.Buffer;
        }
        #endregion
        public CMsgAction()
        {
            Buffer = new byte[50];
            Writer.WriteUInt16((ushort)(Buffer.Length - 8), 0, Buffer);
            Writer.WriteUInt16(10010, 2, Buffer);
            Writer.WriteUInt32((uint)Time32.timeGetTime().GetHashCode(), 4, Buffer);
        }
        [PacketMethod(10010)]
        public static void HandleAction(byte[] Packet, GameClient client)
        {
            var Action = new CMsgAction();
            Action.Deserialize(Packet);
            switch (Action.SubType)
            {
                case ActionType.SetLocation:
                    {
                        Action.X = client.Player.X = 342;
                        Action.Y = client.Player.Y = 240;
                        Action.dwParam = client.Player.Map.ID = 1002;
                        client.Send(Action);
                        break;
                    }
                case ActionType.SetSurroundings:
                    {
                        client.Send(new CMsgPCNum()
                        {
                            BaseID = client.Player.Map.BaseID,
                            ID = client.Player.Map.ID,
                            Status = 0,
                            Weather = 0
                        });
                        // TODO : WEATHER DETAILS
                        client.Send(Action);
                        break;
                    }
                case ActionType.SwitchPK:
                    {
                        var currentPK = (PKMode)Action.dwParam;
                        client.Player.PKMode = currentPK;
                        client.Send(Action);
                        string MSG = "";
                        switch (currentPK)
                        {
                            case PKMode.Capture:
                                MSG = "Capture mode: Slay monsters, black/blue-name criminals, and cross-server players.";
                                break;
                            case PKMode.PK:
                                MSG = "Free PK mode: you can attack monsters and all Players.";
                                break;
                            case PKMode.Peace:
                                MSG = "Peace mode: You can only attack monsters.";
                                break;
                            case PKMode.Team:
                                MSG = "Team mode: slay monsters, and all other players (including cross-server players) not in your current team or guild.";
                                break;
                            case PKMode.Revenge:
                                MSG = "Revenge mode: Slay your listed enemies, monsters, and cross-server players.";
                                break;
                            case PKMode.Guild:
                                MSG = "Guild mode: Slay monsters, and players in your enemy guilds, and cross-server players.";
                                break;
                            case PKMode.Jiang:
                                MSG = "Jiang Hu mode: Slay Jiang Hu fighters, black/blue-name criminals, and cross-server players.";
                                break;
                        }
                        if (MSG != "")
                            client.Send(new CMsgTalk(Color.Red, ChatType.System, MSG, "SYSTEM", "ALL"));
                        break;
                    }
                default:
                    Console.WriteLine("Unknown CMsgAction type : {0}", (uint)Action.SubType);
                    break;
            }
        }
    }
}
