using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountServer.Packets
{
    public class MsgConnectEx
    {
        public enum RejectionCode : uint
        {
            InvalidInfo = 1,
            Ready = 2,
            Banned = 25,
            WrongAccount = 57,
            ServersNotConfigured = 59
        }
        public static byte[] Rejected(RejectionCode Reason)
        {
            byte[] ConnectedEx = new byte[32];
            Writer.WriteUInt16((UInt16)(ConnectedEx.Length), 0, ConnectedEx);
            Writer.WriteUInt16(1055, 2, ConnectedEx);
            Writer.WriteUInt32((UInt32)Reason, 8, ConnectedEx);
            return ConnectedEx;
        }
        public static byte[] Verified(UInt32 Identifier, string GameIP, Int32 GamePort)
        {
            byte[] ConnectedEx = new byte[32];
            Writer.WriteUInt16((UInt16)(ConnectedEx.Length), 0, ConnectedEx);
            Writer.WriteUInt16(1055, 2, ConnectedEx);
            Writer.WriteUInt32(Identifier, 4, ConnectedEx);
            Writer.WriteUInt32(Identifier, 8, ConnectedEx);
            Writer.WriteString(GameIP, 12, ConnectedEx);
            Writer.WriteInt32(GamePort, 28, ConnectedEx);
            return ConnectedEx;
        }
    }
}
