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

            string createdKey = PadKey(source, key);

            string encrypted = conv.Xor(conv.StringToHex(source), conv.StringToHex(createdKey));

            return encrypted;
        }

        private string PadKey(string source, string key)
        {
            StringBuilder s = new StringBuilder();
            int pos = 0;
            while (pos < source.Length)
            {
                s.Append(key);
                pos += key.Length;
            }

            string createdKey = s.ToString().Substring(0, source.Length);
            return createdKey;
        }

        public Dictionary<char, string> TryDecrypt(string hexSource)
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

        public char TryDecryptAndFindKey(string hexSource)
        {
            CharacterCounter cc = new CharacterCounter();
            return cc.FindKey(this.TryDecrypt(hexSource));
        }

        public string TryDecryptFile(string location)
        {
            CharacterCounter cc = new CharacterCounter();
            string[] lines = File.ReadAllLines(location);
            foreach (string line in lines)
            {
                var decrypted = this.TryDecrypt(line);
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

        public string[] BreakXorFile(string location, int startKeySize = 2, int endKeySize = 60, int numberOfBlocks = 2)
        {
            int take = 10;

            CharacterCounter cc = new CharacterCounter();
            Converter conv = new Converter();
            HammingDistance hd = new HammingDistance();

            string source = conv.Base64ToHex(String.Join("", File.ReadAllLines(location)));
            var possibleKeySizes = hd.FindDistancePerKeySize(startKeySize, endKeySize, source, numberOfBlocks).OrderBy(x => x.Value).Select(x => x.Key).Take(take);

            foreach(var possibleKeySize in possibleKeySizes)
            {
                StringBuilder sb = new StringBuilder();

                var transposedBlocks = CreateAndTransposeBlocks(source, possibleKeySize);
                var decryptedBlocks = new List<string>();

                bool foundOne = false;

                foreach (var block in transposedBlocks)
                {
                    var decrypted = this.TryDecrypt(block);
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
