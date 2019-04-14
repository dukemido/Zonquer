using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WorldServer.Packets
{
    public class Handler
    {
        public static Dictionary<ushort, MethodInfo> Methods = new Dictionary<ushort, MethodInfo>();
        public static ulong ClientSeal = System.BitConverter.ToUInt64(Encoding.Default.GetBytes("TQClient"), 0);
        public static void HandlePacket(byte[] packet, GameClient client)
        {
            if (packet == null)
                return;
            if (client == null)
                return;
            ushort Length = System.BitConverter.ToUInt16(packet, 0);
            ushort ID = System.BitConverter.ToUInt16(packet, 2);
            if (ClientSeal != System.BitConverter.ToUInt64(packet, Length))
            {
                client.Disconnect();
                return;
            }
            Report(packet);
            if (Methods.ContainsKey(ID))
                Methods[ID].Invoke(null, new object[] { packet, client });
            /*   else
                 Report(packet);*/
        }
        public static void Report(byte[] packet)
        {
            ushort length = BitConverter.ToUInt16(packet, 0);
            ushort identity = BitConverter.ToUInt16(packet, 2);

            // Print the packet and the packet header:
            Console.WriteLine($"Unhandled packet {identity} of length {length}");
            Console.WriteLine();
            for (int index = 0, l = 0; index < length; index++)
            {
                Console.Write("{0:X2} ", packet[index]); l++;
                if (l % 16 == 0) Console.Write("\n");
            }
            Console.WriteLine("\n");
        }
    }
}
