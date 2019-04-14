using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldServer.Base
{
    public struct Time32
    {
        private uint value;
        public uint Value { get { return value; } }
        private static uint lastValue;
        private const string nowDebug =
            "The last value of Time32.Now was greater than the current value generated during this call. " +
            "This is likely due to a reset in the 49.71 days period. " +
            "See http://msdn.microsoft.com/en-us/library/dd757629(VS.85).aspx for more information.";

        public static Time32 Now
        {
            get
            {
                Time32 current = timeGetTime();
                if (lastValue > current.value)
                {
                    //Restart
                }
                lastValue = current.value;
                return current;
            }
        }

        public Time32(int Value)
        {
            value = (uint)Value;
        }
        public Time32(uint Value)
        {
            value = Value;
        }
        public Time32(long Value)
        {
            value = (uint)Value;
        }

        public Time32 AddMilliseconds(int Amount)
        {
            return new Time32(this.value + Amount);
        }
        public int AllMilliseconds
        {
            get
            {
                return this.GetHashCode();
            }
        }

        public Time32 AddSeconds(int Amount)
        {
            return AddMilliseconds(Amount * 1000);
        }
        public int AllSeconds
        {
            get
            {
                return AllMilliseconds / 1000;
            }
        }
        public Time32 AddMinutes(int Amount)
        {
            return AddSeconds(Amount * 60);
        }
        public int AllMinutes
        {
            get
            {
                return AllSeconds / 60;
            }
        }

        public Time32 AddHours(int Amount)
        {
            return AddMinutes(Amount * 60);
        }
        public int AllHours
        {
            get
            {
                return AllMinutes / 60;
            }
        }

        public Time32 AddDays(int Amount)
        {
            return AddHours(Amount * 24);
        }
        public int AllDays
        {
            get
            {
                return AllHours / 24;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Time32)
                return ((Time32)obj == this);
            return base.Equals(obj);
        }
        public override string ToString()
        {
            return value.ToString();
        }
        public override int GetHashCode()
        {
            return (int)value;
        }

        public static bool operator ==(Time32 t1, Time32 t2)
        {
            return (t1.value == t2.value);
        }
        public static bool operator !=(Time32 t1, Time32 t2)
        {
            return (t1.value != t2.value);
        }
        public static bool operator >(Time32 t1, Time32 t2)
        {
            return (t1.value > t2.value);
        }
        public static bool operator <(Time32 t1, Time32 t2)
        {
            return (t1.value < t2.value);
        }
        public static bool operator >=(Time32 t1, Time32 t2)
        {
            return (t1.value >= t2.value);
        }
        public static bool operator <=(Time32 t1, Time32 t2)
        {
            return (t1.value <= t2.value);
        }
        public static Time32 operator -(Time32 t1, Time32 t2)
        {
            return new Time32(t1.value - t2.value);
        }
        public bool Next(int due = 0, int time = 0)
        {
            if (time == 0)
            {
                time = (int)Time32.timeGetTime().Value;
            }
            return this.Value + due <= time;
        }
        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        public static extern Time32 timeGetTime();

        public ulong Get64HashCode()
        {
            return (ulong)Environment.TickCount;
        }
    }
}
