using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountServer.Packets
{
    public class AccountStructure
    {
        public string Username,
            Password,
            Server,
            MacAddress;
    }
    public class MsgAccountSRP6Ex
    {

        public static AccountStructure Deserialize(byte[] Packet)
        {
            var acc = new AccountStructure();
            if (BitConverter.ToUInt16(Packet, 0) == 276)
            {
                var MS = new MemoryStream(Packet);
                var BR = new BinaryReader(MS);
                BR.ReadUInt16();
                BR.ReadUInt16();
                acc.Username = Encoding.Default.GetString(BR.ReadBytes(16)).Replace("\0", "");
                BR.ReadBytes(112);
                acc.Password = Encoding.Default.GetString(BR.ReadBytes(16));
                BR.ReadBytes(112);
                acc.Server = Encoding.Default.GetString(BR.ReadBytes(16)).Replace("\0", "");
                BR.Close();
                MS.Close();
            }
            return acc;
        }
    }
}
