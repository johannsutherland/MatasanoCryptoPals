﻿using System;
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

            char[] foundCharacters = new char[blockSize];
            char[] completed = new char[unknownString.Decode().Length];
            int completedBlocks = 0;
            int lastPos = 0;

            while (true)
            {
                for (int pos = 0; pos < blockSize; pos++)
                {
                    string message = new String('A', blockSize - pos - 1);
                    Base64 encryptedMessage = this.Encrypt(message, completedBlocks * blockSize);

                    bool found = false;

                    for (int character = 0; character < 256; character++)
                    {
                        string updatedMessage = message + String.Join("", foundCharacters.Take(pos)) + (char)character;
                        Base64 updatedEncryptedMessage = this.Encrypt(updatedMessage, completedBlocks * blockSize);

                        if (updatedEncryptedMessage.Decode().Substring(0, blockSize) ==
                            encryptedMessage.Decode().Substring(0, blockSize))
                        {
                            foundCharacters[pos] = (char)character;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        lastPos = pos - 1;
                        break;
                    }
                }

                if (foundCharacters.Length + (completedBlocks * blockSize) > completed.Length)
                {
                    foundCharacters.Take(lastPos).ToArray().CopyTo(completed, completedBlocks * blockSize);
                    return new String(completed);
                }
                else
                {
                    foundCharacters.ToArray().CopyTo(completed, completedBlocks * blockSize);
                }
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
