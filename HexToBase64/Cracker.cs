using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matasano
{
    public class Cracker
    {
        private readonly HammingDistance hammingDistance;
        private readonly EncryptionOracle eo;
        private readonly AESCipherHelper helper;

        private readonly Base64 unknownString;

        public Cracker(Base64 unknownString)
        {
            this.hammingDistance = new HammingDistance();
            this.eo = new EncryptionOracle();
            this.helper = new AESCipherHelper();
            this.unknownString = unknownString;
        }

        public string Break()
        {
            int blockSize = this.FindBlockSize();

            if (!this.IsECB(blockSize))
            {
                throw new Exception("Must be ECB");
            }

            StringBuilder foundCharacters = new StringBuilder();
            int blocksCompleted = 0;
            int base64BlockSize = 170;

            for (int i = 1; i < blockSize; i++)
            {
                string message = new String('A', blockSize - i);
                var encrypted = new Dictionary<Base64, string>();
                Base64 encryptedMessage = this.Encrypt(message);

                for (int character = 0; character < 256; character++)
                {
                    string updatedMessage = message + foundCharacters.ToString() + (char)character;
                    Base64 updatedEncryptedMessage = this.Encrypt(updatedMessage);
                    encrypted.Add(updatedEncryptedMessage, updatedMessage);

                    if (updatedEncryptedMessage.ToString()
                            .Substring(base64BlockSize * blocksCompleted, base64BlockSize * (blocksCompleted + 1)) 
                        == encryptedMessage.ToString()
                            .Substring(base64BlockSize * blocksCompleted, base64BlockSize * (blocksCompleted + 1)))
                    {
                        foundCharacters.Append((char)character);
                        break;
                    }
                }
            }

            return foundCharacters.ToString();
        }

        public int FindBlockSize()
        {
            var aggregated = new Dictionary<int, float>();
            int start = 1;
            int end = 200;
            int blocks = 2;

            for (int i = 0; i < 100; i++)
            {
                Base64 encrypted = eo.EncryptConsistentKey(new String('A', i), unknownString);
                HammingDistance hd = new HammingDistance();
                var keys = hd.FindDistancePerKeySize(start, end, encrypted.ToString(), blocks).OrderBy(x => x.Value);
                foreach (var kvp in keys)
                {
                    if (aggregated.ContainsKey(kvp.Key))
                    {
                        if (aggregated[kvp.Key] > kvp.Value)
                        {
                            aggregated[kvp.Key] = kvp.Value;
                        }
                    }
                    else
                    {
                        aggregated.Add(kvp.Key, kvp.Value);
                    }
                }
            }

            var possibleKey = aggregated.OrderBy(x => x.Value).Take(1).Single();

            return possibleKey.Key * blocks;
        }

        public bool IsECB(int blockSize)
        {
            Base64 encrypted = eo.EncryptConsistentKey(new String('A', blockSize), unknownString);
            return helper.IsECB(encrypted.Decode());
        }

        public Base64 Encrypt(string craftedBlock)
        {
            return eo.EncryptConsistentKey(craftedBlock, unknownString);
        }
    }
}
