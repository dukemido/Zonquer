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
    public class CMsgConnect
    {
        #region PacketProperty
        byte[] Buffer;
        public uint Identifier
        {
            get { return BitConverter.ToUInt32(Buffer, 4); }
            set { Writer.WriteUInt32(value, 4, Buffer); }
        }
        public uint Type
        {
            get
            {
                return (uint)BitConverter.ToUInt32(Buffer, 8);
            }
            set
            {
                Writer.WriteUInt32((byte)value, 8, Buffer);
            }
        }
        public void Deserialize(byte[] buffer)
        {
            this.Buffer = buffer;
        }
        #endregion


        [PacketMethod(1052)]
        public static void HandleConnect(byte[] packet, GameClient client)
        {
            if (client.Action == 1)
            {
                CMsgConnect connect = new CMsgConnect();
                connect.Deserialize(packet);
                AppendConnect(connect, client);
            }
            else
                client.Disconnect();
        }

        private static void AppendConnect(CMsgConnect connect, GameClient client)
        {
            client.CreatedUID = connect.Identifier;
            GameClient confirmationClient;
            // Prevent logged on clients to login again.
            if (client.MyServer.ConfirmationPool.TryRemove(connect.Identifier, out confirmationClient)
                || client.MyServer.GamePool.TryRemove(connect.Identifier, out confirmationClient))
            {
                confirmationClient.Disconnect();
                client.Send(new CMsgTalk(Color.Orange, ChatType.Dialog, "You're already logged in.", "", "ALLUSERS"));
                client.Disconnect();
                return;
            }
            if (!client.IsCreating)
                client.MyServer.ConfirmationPool.TryAdd(connect.Identifier, client);
            else
                client.IsCreating = false;
            if (!PlayerTable.Load(client.CreatedUID, ref client.Player, client.MyServer))
            {
                client.IsCreating = true;
                client.Send(new CMsgTalk(Color.Orange, ChatType.Dialog, "NEW_ROLE", "", "ALLUSERS"));
                return;
            }
            // BELOW IS ONLY IF THE ACCOUNT IS VALID
            client.Send(new CMsgTalk(Color.Orange, ChatType.Dialog, "ANSWER_OK", "", "ALLUSERS"));
            client.Send(new CMsgUserInfo(client.Player));
            client.MyServer.ConfirmationPool.TryRemove(connect.Identifier, out confirmationClient);
        }
    }
}
