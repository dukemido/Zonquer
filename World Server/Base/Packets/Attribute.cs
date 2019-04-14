using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WorldServer.Base
{
    public class Attributes
    {
        public static void Load()
        {
            var assembly = Assembly.GetCallingAssembly();
            foreach (var types in assembly.GetTypes())
            {
                var methods = types.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var method in methods)
                {
                    foreach (var attribute in method.GetCustomAttributes(true))
                    {
                        var Attr = attribute as PacketMethodAttribute;
                        if (Attr != null)
                        {
                            var ID = Attr.PacketID;
                            Packets.Handler.Methods.Add(ID, method);
                        }
                    }
                }
            }
        }
    }
}
