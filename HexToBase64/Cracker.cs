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
        private readonly Cipher cipher;
        private readonly Converter converter;
        private readonly CharacterCounter characterCounter;
        private readonly HammingDistance hammingDistance;

        public Cracker(Cipher cipher)
        {
            this.cipher = cipher;
            this.converter = new Converter();
            this.characterCounter = new CharacterCounter();
            this.hammingDistance = new HammingDistance();
        }

        public string[] BreakXorFile(string location, int startKeySize = 2, int endKeySize = 60, int numberOfBlocks = 2)
        {
            int take = 10;

            string source = converter.Base64ToHex(String.Join("", File.ReadAllLines(location)));
            var possibleKeySizes = hammingDistance.FindDistancePerKeySize(startKeySize, endKeySize, source, numberOfBlocks).OrderBy(x => x.Value).Select(x => x.Key).Take(take);

            foreach (var possibleKeySize in possibleKeySizes)
            {
                StringBuilder sb = new StringBuilder();

                var transposedBlocks = characterCounter.CreateAndTransposeBlocks(source, possibleKeySize);
                var decryptedBlocks = new List<string>();

                bool foundOne = false;

                foreach (var block in transposedBlocks)
                {
                    var decrypted = cipher.TryDecrypt(block);
                    var c = characterCounter.FindKey(decrypted);
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
