using System.Collections.Generic;
using System.Linq;
using System.Text;

using Matasano.Helper;

namespace Matasano.Cracker
{
    public class XorCracker
    {
        private HammingDistance hammingDistance;
        private CharacterCounter characterCounter;
        private XorCrackerHelper crackerHelper;

        public XorCracker()
        {
            hammingDistance = new HammingDistance();
            characterCounter = new CharacterCounter();
            crackerHelper = new XorCrackerHelper();
        }

        public string[] BreakXorFile(Base64 data, int startKeySize = 2, int endKeySize = 60, int numberOfBlocks = 2)
        {
            int take = 10;

            Hex source = data;
            var possibleKeySizes = hammingDistance.FindDistancePerKeySize(startKeySize, endKeySize, source.ToString(), numberOfBlocks).OrderBy(x => x.Value).Select(x => x.Key).Take(take);

            foreach (var possibleKeySize in possibleKeySizes)
            {
                StringBuilder sb = new StringBuilder();

                var transposedBlocks = source.CreateAndTransposeBlocks(possibleKeySize);
                var decryptedBlocks = new List<string>();

                bool foundOne = false;

                foreach (var block in transposedBlocks)
                {
                    var decrypted = crackerHelper.TryDecrypt(block);
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
