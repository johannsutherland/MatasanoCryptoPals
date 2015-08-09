using System.Collections.Generic;
using System.Linq;
using System.Text;

using Matasano.Helper;
using System.IO;
using System;
using Matasano.Cipher.Xor;

namespace Matasano.Cracker
{
    public class XorCracker
    {
        private readonly XorCipher xorCipher;

        public XorCracker()
        {
            this.xorCipher = new XorCipher();
        }

        public string[] BreakXorFile(Base64 data, int startKeySize = 2, int endKeySize = 60, int numberOfBlocks = 2)
        {
            int take = 10;

            Hex source = data;
            var possibleKeySizes = HammingDistance.FindDistancePerKeySize(startKeySize, endKeySize, source.ToString(), numberOfBlocks).OrderBy(x => x.Value).Select(x => x.Key).Take(take);

            foreach (var possibleKeySize in possibleKeySizes)
            {
                StringBuilder sb = new StringBuilder();

                var transposedBlocks = source.CreateAndTransposeBlocks(possibleKeySize);
                var decryptedBlocks = new List<string>();

                bool foundOne = false;

                foreach (var block in transposedBlocks)
                {
                    var decrypted = this.TryDecrypt(block);
                    var c = CharacterCounter.FindKey(decrypted);
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

        public Dictionary<char, string> TryDecrypt(Hex source)
        {
            var result = new Dictionary<char, string>();

            foreach (char c in CharacterCounter.GetAlphabet())
            {
                string s = new string(c, source.Length);
                string decrypted = xorCipher.Decrypt(source, s);

                result.Add(c, decrypted);
            }

            return result;
        }

        public char TryDecryptAndFindKey(Hex source)
        {
            return CharacterCounter.FindKey(this.TryDecrypt(source));
        }

        public string TryDecryptFile(string location)
        {
            string[] lines = File.ReadAllLines(location);
            foreach (string line in lines)
            {
                var decrypted = this.TryDecrypt(new Hex(line));
                char c = CharacterCounter.FindKey(decrypted);
                if (c != '\0')
                {
                    return decrypted[c];
                }
            }
            return String.Empty;
        }
    }
}
