﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WorldServer.Base
{
    public class Writer
    {
        public static void WriteInt32(int arg, int offset, byte[] buffer)
        {
            if (buffer == null) return;
            if (offset > buffer.Length - 1) return;
            if (buffer.Length >= offset + sizeof(uint))
            {
                unsafe
                {
                    fixed (byte* Buffer = buffer)
                        *((int*)(Buffer + offset)) = arg;
                }
            }
        }
        public static void Write(string arg, int offset, byte[] buffer)
        {
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            byte[] argEncoded = Encoding.Default.GetBytes(arg);
            if (buffer.Length >= offset + arg.Length)
                Array.Copy(argEncoded, 0, buffer, offset, arg.Length);
        }
        public static void Write(byte arg, int offset, byte[] buffer)
        {
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            buffer[offset] = arg;
        }
        public static void Write(bool arg, int offset, byte[] buffer)
        {
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            Write(arg == true ? (byte)1 : (byte)0, offset, buffer);
        }
        public static void WriteUInt32(uint arg, int offset, byte[] buffer)
        {
            if (buffer == null) return;
            if (offset > buffer.Length - 1) return;
            if (buffer.Length >= offset + sizeof(uint))
            {
                unsafe
                {
                    fixed (byte* Buffer = buffer)
                        *((uint*)(Buffer + offset)) = arg;
                }
            }
        }
        public static void WriteStringList(string[] arg, int offset, byte[] buffer)
        {
            if (arg == null)
                return;
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            buffer[offset] = (byte)arg.Length;
            offset++;
            foreach (string str in arg)
            {
                buffer[offset] = (byte)str.Length;
                WriteString(str, offset + 1, buffer);
                offset += str.Length + 1;
            }
        }
        public static void Ulong(ulong arg, int offset, byte[] buffer)
        {
            if (buffer == null) return;
            if (offset > buffer.Length - 1) return;
            if (buffer.Length >= offset + sizeof(ulong))
            {
                unsafe
                {
                    fixed (byte* Buffer = buffer)
                    {
                        *((ulong*)(Buffer + offset)) = arg;
                    }
                }
            }
        }
        public static void WriteStringList(List<string> arg, int offset, byte[] buffer)
        {
            if (arg == null)
                return;
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            buffer[offset] = (byte)arg.Count;
            offset++;
            foreach (string str in arg)
            {
                buffer[offset] = (byte)str.Length;
                WriteString(str, offset + 1, buffer);
                offset += str.Length + 1;
            }
        }
        public static void Ushort(ushort arg, int offset, byte[] buffer)
        {
            if (buffer == null) return;
            if (offset > buffer.Length - 1) return;
            if (buffer.Length >= offset + sizeof(ushort))
            {
                unsafe
                {
                    fixed (byte* Buffer = buffer)
                    {
                        *((ushort*)(Buffer + offset)) = arg;
                    }
                }
            }
        }
        public static void WriteByte(byte arg, int offset, byte[] buffer)
        {
            if (buffer == null) return;
            if (offset > buffer.Length - 1) return;
            buffer[offset] = arg;
        }
        public static void WriteUint(uint arg, int offset, byte[] buffer)
        {
            if (buffer == null) return;
            if (offset > buffer.Length - 1) return;
            if (buffer.Length >= offset + sizeof(uint))
            {
                unsafe
                {
                    fixed (byte* Buffer = buffer)
                    {
                        *((uint*)(Buffer + offset)) = arg;
                    }
                }
            }
        }
        public static void WriteUshort(ushort arg, int offset, byte[] buffer)
        {
            if (buffer == null) return;
            if (offset > buffer.Length - 1) return;
            if (buffer.Length >= offset + sizeof(ushort))
            {
                unsafe
                {
                    fixed (byte* Buffer = buffer)
                    {
                        *((ushort*)(Buffer + offset)) = arg;
                    }
                }
            }
        }
        public static void WriteString(string arg, int offset, byte[] buffer)
        {
            if (buffer == null)
            {
                return;
            }
            if (offset > buffer.Length - 1)
            {
                return;
            }
            if (buffer.Length >= offset + arg.Length)
            {
                unsafe
                {
#if UNSAFE
                    fixed (byte* Buffer = buffer)
                    {
                        ushort i = 0;
                        while (i < arg.Length)
                        {
                            *((byte*)(Buffer + offset + i)) = (byte)arg[i];
                            i++;
                        }
                    }
#else
                    ushort i = 0;
                    while (i < arg.Length)
                    {
                        buffer[(ushort)(i + offset)] = (byte)arg[i];
                        i = (ushort)(i + 1);
                    }
#endif
                }
            }
        }
        public static void WriteUInt16(ushort arg, int offset, byte[] buffer)
        {
            if (buffer == null) return;
            if (offset > buffer.Length - 1) return;
            if (buffer.Length >= offset + sizeof(ushort))
            {
                unsafe
                {
                    fixed (byte* Buffer = buffer)
                        *((ushort*)(Buffer + offset)) = arg;
                }
            }
        }
        public static void WriteUInt64(ulong arg, int offset, byte[] buffer)
        {
            if (buffer == null) return;
            if (offset > buffer.Length - 1) return;
            if (buffer.Length >= offset + sizeof(ulong))
            {
                unsafe
                {
                    fixed (byte* Buffer = buffer)
                        *((ulong*)(Buffer + offset)) = arg;
                }
            }
        }
        public static void WriteInt64(long arg, int offset, byte[] buffer)
        {
            if (buffer == null) return;
            if (offset > buffer.Length - 1) return;
            if (buffer.Length >= offset + sizeof(long))
            {
                unsafe
                {
                    fixed (byte* Buffer = buffer)
                        *((long*)(Buffer + offset)) = arg;
                }
            }
        }
        public static void Write(ushort arg, int offset, byte[] buffer)
        {
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            if (buffer.Length >= offset + sizeof(ushort))
            {
                unsafe
                {
#if UNSAFE
                    fixed (byte* Buffer = buffer)
                    {
                        *((ushort*)(Buffer + offset)) = arg;
                    }
#else
                    buffer[offset] = (byte)arg;
                    buffer[offset + 1] = (byte)(arg >> 8);
#endif
                }
            }
        }
        public static void Write(uint arg, int offset, byte[] buffer)
        {
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            if (buffer.Length >= offset + sizeof(uint))
            {
                unsafe
                {
#if UNSAFE
                    fixed (byte* Buffer = buffer)
                    {
                        *((uint*)(Buffer + offset)) = arg;
                    }
#else
                    buffer[offset] = (byte)arg;
                    buffer[offset + 1] = (byte)(arg >> 8);
                    buffer[offset + 2] = (byte)(arg >> 16);
                    buffer[offset + 3] = (byte)(arg >> 24);
#endif
                }
            }
        }
        public static unsafe void Write(decimal arg, int offset, byte[] Buffer)
        {
            if (Buffer == null)
                return;
            if (offset > Buffer.Length - 1)
                return;
            fixed (byte* buffer = Buffer)
            {
                if (arg.GetType() == typeof(decimal))
                {
                    *((decimal*)(buffer + offset)) = arg;
                }
            }
        }
        public static void Write(ulong arg, int offset, byte[] buffer)
        {
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            if (buffer.Length >= offset + sizeof(ulong))
            {
                unsafe
                {
#if UNSAFE
                    fixed (byte* Buffer = buffer)
                    {
                        *((ulong*)(Buffer + offset)) = arg;
                    }
#else
                    buffer[offset] = (byte)(arg);
                    buffer[offset + 1] = (byte)(arg >> 8);
                    buffer[offset + 2] = (byte)(arg >> 16);
                    buffer[offset + 3] = (byte)(arg >> 24);
                    buffer[offset + 4] = (byte)(arg >> 32);
                    buffer[offset + 5] = (byte)(arg >> 40);
                    buffer[offset + 6] = (byte)(arg >> 48);
                    buffer[offset + 7] = (byte)(arg >> 56);
#endif
                }
            }
        }
        public static void Write(int arg, int offset, byte[] buffer)
        {
            if (buffer == null)
            {
                return;
            }
            if (offset > buffer.Length - 1)
            {
                return;
            }
            if (buffer.Length >= offset + sizeof(uint))
            {
                unsafe
                {
#if UNSAFE
                    fixed (byte* Buffer = buffer)
                    {
                        *((int*)(Buffer + offset)) = arg;
                    }
#else
                    buffer[offset] = (byte)(arg);
                    buffer[offset + 1] = (byte)(arg >> 8);
                    buffer[offset + 2] = (byte)(arg >> 16);
                    buffer[offset + 3] = (byte)(arg >> 24);
#endif
                }
            }
        }
        public static void Write(List<string> arg, int offset, byte[] buffer)
        {
            if (arg == null)
                return;
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            buffer[offset] = (byte)arg.Count;
            offset++;
            foreach (string str in arg)
            {
                buffer[offset] = (byte)str.Length;
                Writer.Write(str, offset + 1, buffer);
                offset += str.Length + 1;
            }
        }
        public static void Write(string[] arg, int offset, byte[] buffer)
        {
            if (arg == null)
                return;
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            buffer[offset] = (byte)arg.Length;
            offset++;
            foreach (string str in arg)
            {
                buffer[offset] = (byte)str.Length;
                Writer.Write(str, offset + 1, buffer);
                offset += str.Length + 1;
            }
        }
        public static void Byte(byte arg, int offset, byte[] buffer)
        {
            if (buffer == null) return;
            if (offset > buffer.Length - 1) return;
            buffer[offset] = arg;
        }
        public static void WriteBoolean(bool arg, int offset, byte[] buffer)
        {
            if (buffer == null) return;
            if (offset > buffer.Length - 1) return;
            WriteByte(arg == true ? (byte)1 : (byte)0, offset, buffer);
        }
      
        public static void WriteStringWithLength(string arg, int offset, byte[] buffer)
        {
            if (buffer == null)
            {
                return;
            }
            if (offset > buffer.Length - 1)
            {
                return;
            }
            int till = buffer.Length - offset;
            till = Math.Min(arg.Length, till);
            buffer[offset] = (byte)arg.Length;
            offset++;
            ushort i = 0;
            var bytes = Encoding.Default.GetBytes(arg);
            while (i < till)
            {
                buffer[(ushort)(i + offset)] = bytes[i];
                i = (ushort)(i + 1);
            }
        }
        public static void WriteWithLength(string arg, int offset, byte[] buffer)
        {
            if (buffer == null)
            {
                return;
            }
            if (offset > buffer.Length - 1)
            {
                return;
            }
            int till = buffer.Length - offset;
            till = Math.Min(arg.Length, till);
            buffer[offset] = (byte)arg.Length;
            offset++;
            ushort i = 0;
            var bytes = Encoding.Default.GetBytes(arg);
            while (i < till)
            {
                buffer[(ushort)(i + offset)] = bytes[i];
                i = (ushort)(i + 1);
            }
        }
    }
}