using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldServer.Base
{
    public class PacketMethodAttribute : Attribute
    {
        public ushort PacketID { get; set; }
        public PacketMethodAttribute(ushort PacketID)
        {
            this.PacketID = PacketID;
        }

        public PacketMethodAttribute()
        {
        }
    }
}
