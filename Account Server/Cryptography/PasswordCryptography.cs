using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace AccountServer.Cryptography
{
    public unsafe class PasswordCryptography
    {
        static UInt32 LeftRotate(UInt32 var, UInt32 offset)
        {
            UInt32 tmp1, tmp2;
            offset &= 0x1f;
            tmp1 = var >> (int)(32 - offset);
            tmp2 = var << (int)offset;
            tmp2 |= tmp1;
            return tmp2;
        }
        static UInt32 RightRotate(UInt32 var, UInt32 offset)
        {
            UInt32 tmp1, tmp2;
            offset &= 0x1f;
            tmp1 = var << (int)(32 - offset);
            tmp2 = var >> (int)offset;
            tmp2 |= tmp1;
            return tmp2;
        }

        static uint[] key = new uint[] {
                0xEBE854BC, 0xB04998F7, 0xFFFAA88C,
                0x96E854BB, 0xA9915556, 0x48E44110,
                0x9F32308F, 0x27F41D3E, 0xCF4F3523,
                0xEAC3C6B4, 0xE9EA5E03, 0xE5974BBA,
                0x334D7692, 0x2C6BCF2E, 0xDC53B74,
                0x995C92A6, 0x7E4F6D77, 0x1EB2B79F,
                0x1D348D89, 0xED641354, 0x15E04A9D,
                0x488DA159, 0x647817D3, 0x8CA0BC20,
                0x9264F7FE, 0x91E78C6C, 0x5C9A07FB,
                0xABD4DCCE, 0x6416F98D, 0x6642AB5B
        };
        public static string EncryptPassword(string password)
        {
            UInt32 tmp1, tmp2, tmp3, tmp4, A, B, chiperOffset, chiperContent;

            byte[] plain = new byte[16];
            Encoding.ASCII.GetBytes(password, 0, password.Length, plain, 0);

            MemoryStream mStream = new MemoryStream(plain);
            BinaryReader bReader = new BinaryReader(mStream);
            UInt32[] pSeeds = new UInt32[4];
            for (int i = 0; i < 4; i++) pSeeds[i] = bReader.ReadUInt32();
            bReader.Close();

            chiperOffset = 7;

            byte[] encrypted = new byte[plain.Length];
            MemoryStream eStream = new MemoryStream(encrypted);
            BinaryWriter bWriter = new BinaryWriter(eStream);

            for (int j = 0; j < 2; j++)
            {
                tmp1 = tmp2 = tmp3 = tmp4 = 0;
                tmp1 = key[5];
                tmp2 = pSeeds[j * 2];
                tmp3 = key[4];
                tmp4 = pSeeds[j * 2 + 1];

                tmp2 += tmp3;
                tmp1 += tmp4;

                A = B = 0;

                for (int i = 0; i < 12; i++)
                {
                    chiperContent = 0;
                    A = LeftRotate(tmp1 ^ tmp2, tmp1);
                    chiperContent = key[chiperOffset + i * 2 - 1];
                    tmp2 = A + chiperContent;

                    B = LeftRotate(tmp1 ^ tmp2, tmp2);
                    chiperContent = key[chiperOffset + i * 2];
                    tmp1 = B + chiperContent;
                }

                bWriter.Write(tmp2);
                bWriter.Write(tmp1);
            }
            bWriter.Close();

            return ASCIIEncoding.Default.GetString(encrypted); // i have allot of servers that are almost done just not finished worked on them with the owner of elemnt co but for now change that back to 8 nd start it .

        }

    }
}
