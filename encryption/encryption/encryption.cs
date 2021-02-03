using System;
using System.Collections.Generic;
using System.Text;

namespace encryption
{

    public class Encryption
    {

        public byte[] EncryptByte(byte[] bytes, byte key)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                int change = i + key;
                while (change + bytes[i] > 255) change -= 256;
                bytes[i] += (byte)change;
            }
            return bytes;
        }

        public byte[] DecryptByte(byte[] bytes, byte key)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                int change = i + key;
                while (change - bytes[i] < 0) change += 256;
                bytes[i] -= (byte)change;
            }
            return bytes;

        }

        public void PrintBytesUTF8(byte[] bytes)
        {
            string toPrint = Encoding.UTF8.GetString(bytes);
            Console.WriteLine(toPrint);
        }
    }
}
