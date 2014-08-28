using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matasano
{
    public class XorCipher
    {
        public string Encrypt(string source, string key)
        {
            Converter conv = new Converter();

            StringBuilder s = new StringBuilder();
            int pos = 0;
            while (pos < source.Length)
            {
                s.Append(key);
                pos += key.Length;
            }

            string createdKey = s.ToString().Substring(0, source.Length);

            string encrypted = conv.Xor(conv.StringToHex(source), conv.StringToHex(createdKey));

            return encrypted;
        }

        public Dictionary<char, string> Decrypt(string hexSource)
        {
            CharacterCounter cc = new CharacterCounter();
            Converter conv = new Converter();

            var result = new Dictionary<char, string>();

            foreach (char c in cc.GetAlphabet())
            {
                string s = new string(c, hexSource.Length / 2);
                string decrypted = conv.HexToString(conv.Xor(hexSource, conv.StringToHex(s)));

                result.Add(c, decrypted);
            }

            return result;
        }

        public char DecryptAndFindKey(string hexSource)
        {
            CharacterCounter cc = new CharacterCounter();
            return cc.FindKey(this.Decrypt(hexSource));
        }

        public string DecryptFile(string location)
        {
            CharacterCounter cc = new CharacterCounter();
            string[] lines = File.ReadAllLines(location);
            foreach (string line in lines)
            {
                var decrypted = this.Decrypt(line);
                char c = cc.FindKey(decrypted);
                if (c != '\0')
                {
                    return decrypted[c];
                }
            }
            return String.Empty;
        }

        public string[] CreateAndTransposeBlocks(string source, int keySize)
        {
            string[] blocks = source.SplitByLength(keySize).ToArray();
            string[] transposed = new string[keySize / 2];

            for (int i = 0; i < transposed.Length; i++)
            {
                foreach (string block in blocks)
                {
                    if (2 * i + 1 < block.Length)
                    {
                        transposed[i] += block[2 * i].ToString() + block[2 * i + 1].ToString();
                    }
                }
            }

            return transposed;
        }

        public Dictionary<int, float> FindDistancePerKeySize(int startKeySize, int endKeySize, string source, int numberOfBlocks = 2)
        {
            if (numberOfBlocks % 2 != 0)
            {
                throw new ArgumentException("Number of blocks must be divisable by 2", "numberOfBlocks");
            }

            var result = new Dictionary<int, float>();

            Converter cv = new Converter();
            for (int keySize = startKeySize; keySize <= endKeySize; keySize++)
            {
                if (numberOfBlocks * keySize > source.Length)
                {
                    break;
                }

                string[] str = new string[numberOfBlocks];
                for (int i = 0; i < numberOfBlocks; i++)
                {
                    str[i] = source.Substring(i * keySize, keySize);
                }
                
                int totalDistance = 0;
                for (int i = 0; i * 2 < str.Length; i++)
                {
                    totalDistance += cv.HammingDistance(str[2 * i], str[2 * i + 1]);
                }

                result.Add(keySize, ((float)totalDistance / (float)keySize));
            }

            return result;
        }

        public string[] BreakXorFile(string location, int startKeySize = 2, int endKeySize = 60, int numberOfBlocks = 2)
        {
            int take = 10;

            CharacterCounter cc = new CharacterCounter();
            Converter conv = new Converter();

            string source = conv.Base64ToHex(String.Join("", File.ReadAllLines(location)));
            var possibleKeySizes = this.FindDistancePerKeySize(startKeySize, endKeySize, source, numberOfBlocks).OrderBy(x => x.Value).Select(x => x.Key).Take(take);

            foreach(var possibleKeySize in possibleKeySizes)
            {
                StringBuilder sb = new StringBuilder();

                var transposedBlocks = CreateAndTransposeBlocks(source, possibleKeySize);
                var decryptedBlocks = new List<string>();

                bool foundOne = false;

                foreach (var block in transposedBlocks)
                {
                    var decrypted = this.Decrypt(block);
                    var c = cc.FindKey(decrypted);
                    if (c == '\0')
                    {
                        if (foundOne)
                        {
                            sb.Append('?');
                            decryptedBlocks.Add(decrypted['A']);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        foundOne = true;
                        sb.Append(c);
                        decryptedBlocks.Add(decrypted[c]);
                    }
                }

                string key = sb.ToString();

                if (key.Length > 0)
                {
                    StringBuilder decrypted = new StringBuilder();
                    for (int i = 0; i < decryptedBlocks[0].Length; i++)
                    {
                        foreach (string block in decryptedBlocks)
                        {
                            if (i < block.Length)
                            {
                                decrypted.Append(block[i]);
                            }
                        }
                    }

                    return new string[] { key, decrypted.ToString() };
                }
            }

            return new string[] { "", "" };
        }
    }
}
