using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldServer.Game;

namespace WorldServer.MapObjects
{
    public enum MapObjects
    {
        Player,
        Monster,
        Npc,
        SobNpc,
        FloorItem,
        ThunderCloud
    }
    public abstract class Entity
    {
        #region Base Properties
        byte[] SpawnPacket;
        public Map Map;
        public MapObjects ObjType
        {
            get;
            private set;
        }
        public ushort X
        {
            get;
            set;
        }
        public ushort Y
        {
            get;
            set;
        }
        public uint UID
        {
            get;
            set;
        }
        public uint Mesh
        {
            get;
            set;
        }
        public ushort Hitpoints
        {
            get;
            set;
        }
        public byte Level
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        #endregion
        public Entity(MapObjects ObjType)
        {
            this.ObjType = ObjType;
        }
    }
}
