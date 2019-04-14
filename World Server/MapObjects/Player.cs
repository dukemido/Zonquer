using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldServer.MapObjects
{
    public enum PKMode : byte
    {
        PK = 0,
        Peace = 1,
        Team = 2,
        Capture = 3,
        Revenge = 4,
        Guild = 5,
        Jiang = 6,
        CS = 7,
        Invade = 8,
        Plander = 9,
        Peacee = 10,
        Union = 11,
    }
    public class Player : Entity
    {
        #region Player Properties
        public Player()
            : base(MapObjects.Player)
        {
            Map = new Game.Map();
        }
        public PKMode PKMode
        {
            get;
            set;
        }
        public ushort Hairstyle
        {
            get;
            set;
        }
        public ushort Face
        {
            get;
            set;
        }
        public ushort Body
        {
            get;
            set;
        }
        public ulong Silvers
        {
            get;
            set;
        }
        public uint ConquerPoints
        {
            get;
            set;
        }
        public uint BoundConquerPoints
        {
            get;
            set;
        }
        public ulong Experience
        {
            get;
            set;
        }
        public ushort Strength
        {
            get;
            set;
        }
        public ushort Agility
        {
            get;
            set;
        }
        public ushort Vitality
        {
            get;
            set;
        }
        public ushort Spirit
        {
            get;
            set;
        }
        public ushort Attributes
        {
            get;
            set;
        }
        public ushort Mana
        {
            get;
            set;
        }
        public ushort PKPoints
        {
            get;
            set;
        }
        public byte VIPLevel
        {
            get;
            set;
        }
        public uint WindWalker
        {
            get;
            set;
        }
        public byte Class
        {
            get;
            set;
        }
        public byte FirstRebornClass
        {
            get;
            set;
        }
        public byte SecondRebornClass
        {
            get;
            set;
        }
        public byte Reborn
        {
            get;
            set;
        }
        public uint QuizPoints
        {
            get;
            set;
        }
        public byte EnlightenPoints
        {
            get;
            set;
        }
        public ushort Country
        {
            get;
            set;
        }
        public string Spouse
        {
            get;
            set;
        }
        #endregion
    }
}
