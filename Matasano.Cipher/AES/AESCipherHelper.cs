using System;
using System.Linq;

namespace Matasano.Cipher.AES
{
    public class AESCipherHelper
    {
        private int blockSizeBits;
        private int blockSize;

        public AESCipherHelper(int blockSize = 16)
        {
            this.blockSizeBits = blockSize * 8;
            this.blockSize = blockSize;
        }

        public bool IsECB(string line)
        {
            var blocks = line.SplitByLength(blockSize);
            var groups = blocks.GroupBy(x => x);
            var repeats = groups.Where(x => x.Count() > 1);

            return (repeats.Count() > 0);
        }

        public string AddPadding(string data)
        {
            if (data.Length % blockSize == 0)
            {
                return data + new String((char)(blockSize), blockSize);
            }
            else
            {
                int paddingLength = blockSize - (data.Length % blockSize);
                return data + new String((char)(paddingLength), paddingLength);
            }
        }

        public string RemovePadding(string data)
        {
            char c = data[data.Length - 1];
            int padding = (int)c;

            if ((padding <= blockSize) && (padding > 0))
            {
                for (int i = data.Length - 1; i >= data.Length - padding; i--)
                {
                    if (data[i] != c)
                    {
                        throw new InvalidPaddingException("Invalid Padding");
                    }
                }

                return data.Substring(0, data.Length - padding);
            }
            else
            {
                throw new InvalidPaddingException("Invalid Padding");
            }
        }

        public Bytes GenerateKey()
        {
            Random random = new Random();
            byte[] key = new byte[blockSize];
            random.NextBytes(key);
            return new Bytes(key);
        }

        public Bytes GenerateRandomLengthKey()
        {
            Random random = new Random();
            byte[] key = new byte[random.Next(1, blockSize)];
            random.NextBytes(key);
            return new Bytes(key);
        }
    }
}
