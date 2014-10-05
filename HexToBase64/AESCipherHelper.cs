using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matasano
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
                return data;
            }
            else
            {
                int paddingLength = blockSize - (data.Length % blockSize);
                return data + new String((char)(paddingLength), paddingLength);
            }
        }

        public string RemovePadding(string data)
        {
            string cleanedData = data.Trim('\0');
            char c = cleanedData[cleanedData.Length - 1];
            int padding = (int)c;

            if ((data.Length - padding > 0) && (data.Substring(data.Length - padding).All(x => x == c || x == '\0')))
            {
                return cleanedData.Substring(0, data.Length - padding);
            }
            else
            {
                return cleanedData;
            }
        }

        public Bytes GenerateKey()
        {
            Random random = new Random();
            byte[] key = new byte[blockSize];
            random.NextBytes(key);
            return new Bytes(key);
        }
    }
}
