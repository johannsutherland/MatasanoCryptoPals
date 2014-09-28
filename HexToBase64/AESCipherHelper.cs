using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matasano
{
    public class AESCipherHelper
    {
        private int blockSize;
        private int keySize;

        public AESCipherHelper(int blockSize = 128, int keySize = 16)
        {
            this.blockSize = blockSize;
            this.keySize = keySize;
        }

        public bool IsECB(string line)
        {
            var blocks = line.SplitByLength(keySize);
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

            if (data.Substring(data.Length - padding).All(x => x == c || x == '\0'))
                return cleanedData.Substring(0, data.Length - padding);
            else
                return cleanedData;
        }

        public Bytes GenerateKey()
        {
            Random random = new Random();
            byte[] key = new byte[16];
            random.NextBytes(key);
            return new Bytes(key);
        }
    }
}
