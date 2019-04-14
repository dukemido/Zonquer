using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldServer.Base;

namespace WorldServer.Packets
{
    public class CMsgPCNum : BasePacket
    {
        byte[] Buffer;
        public CMsgPCNum()
        {
            Buffer = new byte[28];
            Writer.WriteUInt16((ushort)(Buffer.Length - 8), 0, Buffer);
            Writer.WriteUInt16(1110, 2, Buffer);
        }
        public uint ID
        {
            get { return BitConverter.ToUInt32(Buffer, 4); }
            set { Writer.WriteUInt32(value, 4, Buffer); }
        }
        public uint BaseID
        {
            get { return BitConverter.ToUInt32(Buffer, 8); }
            set { Writer.WriteUInt32(value, 8, Buffer); }
        }
        public uint Status
        {
            get { return BitConverter.ToUInt32(Buffer, 12); }
            set { Writer.WriteUInt32(value, 12, Buffer); }
        }
        public uint Weather
        {
            get { return BitConverter.ToUInt32(Buffer, 20); }
            set { Writer.WriteUInt32(value, 20, Buffer); }
        }
        public override byte[] ToArray()
        {
            return Buffer;
        }
    }
}
