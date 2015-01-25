using System;
using System.Collections.Generic;
using System.Linq;

using Matasano.Helper;
using Matasano.Oracle;
using Matasano.Cipher.AES;

namespace Matasano.Cracker
{
    public class AESCracker
    {
        private readonly HammingDistance hammingDistance;
        private readonly AESCipherHelper helper;

        protected IEncryptionOracle eo;
        protected readonly Base64 unknownString;

        public AESCracker(Base64 unknownString)
        {
            this.hammingDistance = new HammingDistance();
            this.eo = new EncryptionOracle();
            this.helper = new AESCipherHelper();
            this.unknownString = unknownString;
        }

        public string Break(int prefixLength = 0)
        {
            int blockSize = this.FindBlockSize();

            if (!this.IsECB(blockSize))
            {
                throw new Exception("Must be ECB");
            }

            char[] foundCharacters = new char[blockSize];
            char[] completed = new char[unknownString.Decode().Length];
            int completedBlocks = 0;
            string padding = String.Empty;
            int start =  0;

            if (prefixLength != 0)
            {
                padding = new String('A', blockSize - prefixLength);
                start = blockSize;
            }

            while (true)
            {
                for (int pos = 0; pos < blockSize; pos++)
                {
                    string message = padding + new String('A', blockSize - pos - 1);
                    Base64 encryptedMessage = this.Encrypt(message, completedBlocks * blockSize);

                    bool found = false;

                    for (int character = 0; character < 256; character++)
                    {
                        string updatedMessage = message + String.Join("", foundCharacters.Take(pos)) + (char)character;
                        Base64 updatedEncryptedMessage = this.Encrypt(updatedMessage, completedBlocks * blockSize);

                        if (updatedEncryptedMessage.Decode().Substring(start, blockSize) ==
                            encryptedMessage.Decode().Substring(start, blockSize))
                        {
                            foundCharacters[pos] = (char)character;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        foundCharacters.Take(pos - 1).ToArray().CopyTo(completed, completedBlocks * blockSize);
                        return new String(completed);
                    }
                }

                foundCharacters.ToArray().CopyTo(completed, completedBlocks * blockSize);
                completedBlocks++;
            }
        }

        public int FindBlockSize()
        {
            var aggregated = new Dictionary<int, float>();
            int start = 10;
            int end = 32;
            int blocks = 4;

            for (int i = start; i < end * 3; i++)
            {
                Base64 encrypted = eo.EncryptConsistentKey(new String('A', i), unknownString);
                HammingDistance hd = new HammingDistance();
                var keys = hd.FindDistancePerKeySize(start, end, encrypted.Decode(), blocks);
                foreach (var kvp in keys)
                {
                    if (aggregated.ContainsKey(kvp.Key))
                    {
                        aggregated[kvp.Key] += kvp.Value;
                    }
                    else
                    {
                        aggregated.Add(kvp.Key, kvp.Value);
                    }
                }
            }

            var possibleKey = aggregated.OrderBy(x => x.Value).Take(1);

            return possibleKey.Select(x => x.Key).Single();
        }

        public bool IsECB(int blockSize)
        {
            Base64 encrypted = eo.EncryptConsistentKey(new String('A', blockSize * 8), unknownString);
            return helper.IsECB(encrypted.Decode());
        }

        public Base64 Encrypt(string craftedBlock, int startIndex = 0)
        {
            return eo.EncryptConsistentKey(craftedBlock, unknownString, startIndex);
        }
    }
}
