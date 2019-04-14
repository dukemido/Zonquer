﻿using System;

namespace AccountServer.Cryptography
{
    public class AuthCryptography
    {
        class CryptCounter
        {
            public CryptCounter()
            {

            }
            public CryptCounter(ushort with)
            {
                m_Counter = with;
            }
            UInt16 m_Counter = 0;

            public byte Key2
            {
                get { return (byte)(m_Counter >> 8); }
            }

            public byte Key1
            {
                get { return (byte)(m_Counter & 0xFF); }
            }

            public void Increment()
            {
                m_Counter++;
            }
        }

        private CryptCounter _decryptCounter;
        private CryptCounter _encryptCounter;
        private static byte[] _cryptKey1;
        private static byte[] _cryptKey2;
        private static bool Decrypt2 = false;

        public static void PrepareAuthCryptography()
        {
            if (_cryptKey1 != null)
            {
                if (_cryptKey1.Length != 0)
                    return;
            }
            _cryptKey1 = new byte[0x100];
            _cryptKey2 = new byte[0x100];
            byte i_key1 = 0x9D;
            byte i_key2 = 0x62;
            for (int i = 0; i < 0x100; i++)
            {
                _cryptKey1[i] = i_key1;
                _cryptKey2[i] = i_key2;
                i_key1 = (byte)((0x0F + (byte)(i_key1 * 0xFA)) * i_key1 + 0x13);
                i_key2 = (byte)((0x79 - (byte)(i_key2 * 0x5C)) * i_key2 + 0x6D);
            }
        }
        public AuthCryptography()
        {
            _encryptCounter = new CryptCounter();
            _decryptCounter = new CryptCounter();
        }
        public void Encrypt(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] ^= (byte)0xAB;
                buffer[i] = (byte)(buffer[i] >> 4 | buffer[i] << 4);
                buffer[i] ^= (byte)(_cryptKey1[_encryptCounter.Key1] ^ _cryptKey2[_encryptCounter.Key2]);
                _encryptCounter.Increment();
            }
        }
        public void Decrypt(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] ^= (byte)0xAB;
                buffer[i] = (byte)(buffer[i] >> 4 | buffer[i] << 4);
                buffer[i] ^= (byte)(_cryptKey2[_decryptCounter.Key2] ^ _cryptKey1[_decryptCounter.Key1]);
                _decryptCounter.Increment();
            }
        }

    }
}
