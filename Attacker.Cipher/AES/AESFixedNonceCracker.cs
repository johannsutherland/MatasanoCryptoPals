using Matasano;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attacker.Cracker.AES
{
    class AESFixedNonceCracker
    {
        readonly int blockSize;

        public AESFixedNonceCracker()
        {
            blockSize = 16;
        }

        public List<string> Crack(Bytes[] encryptedLines)
        {
            var numberOfLines = encryptedLines.Count();
            List<string> crackedLines = new List<string>(numberOfLines);
            Enumerable.Range(0, numberOfLines - 1).ToList().ForEach(x => crackedLines.Add(String.Empty));

            var maximumLength = encryptedLines.Max(x => x.Length);

            for (int i = 0; i < (maximumLength / blockSize) + 1; i++)
            {
                var blocksToCrack = encryptedLines
                    .Select(x => x.SafeSubstring(i * blockSize, blockSize))
                    .ToArray();

                var keystream = GetKeystream(blocksToCrack);
                var crackedBlocks = CrackBlocks(keystream, blocksToCrack);

                for (int crackedBlock = 0; crackedBlock < crackedBlocks.Count; crackedBlock++)
                {
                    crackedLines[crackedBlock] += crackedBlocks[crackedBlock];
                }
            }

            return crackedLines;
        }

        private Bytes GetKeystream(Bytes[] encryptedLines)
        {
            var numberOfLines = encryptedLines.Count();
            var validCharacters = GetValidCharacters();

            List<int> pinnedPositions = new List<int>();
            Bytes plainTextGuess = new Bytes(new String(' ', blockSize));

            int iteration = 0;
            while (pinnedPositions.Count() < blockSize)
            {
                foreach (int position in Enumerable.Range(0, 16).Where(position => !pinnedPositions.Contains(position)))
                {
                    plainTextGuess[position] = (byte)validCharacters[iteration % validCharacters.Count()];
                }

                Bytes potentialKeystream = encryptedLines[0].Xor(plainTextGuess);

                Dictionary<int, int> counter = new Dictionary<int, int>();
                Enumerable.Range(0, blockSize).ToList().ForEach(num => counter.Add(num, 0));

                foreach (var line in encryptedLines.Skip(1))
                {
                    var potentialPlainText = line.Xor(potentialKeystream);

                    for (int c = 0; c < potentialPlainText.Length; c++)
                    {
                        if (validCharacters.Contains((char)potentialPlainText[c]))
                        {
                            counter[c]++;
                        }
                    }
                }

                pinnedPositions.AddRange(counter.Where(
                    kvp => kvp.Value == numberOfLines - (1 + (int)(Math.Floor((decimal)iteration / (1 + validCharacters.Count())))) &&
                    !pinnedPositions.Contains(kvp.Key))
                    .Select(kvp => kvp.Key)
                    .ToList());

                iteration++;
            }

            Bytes keystream = encryptedLines[0].Xor(plainTextGuess);
            return keystream;
        }

        private List<string> CrackBlocks(Bytes keystream, Bytes[] encryptedLines)
        {
            List<string> crackedBlocks = new List<string>();

            foreach (var encryptedLine in encryptedLines.Skip(1))
            {
                var plainText = encryptedLine.Xor(keystream);
                crackedBlocks.Add(plainText.ToString());
            }

            return crackedBlocks;
        }

        private char[] GetValidCharacters()
        {
            var result = Enumerable.Range('A', 'Z' - 'A' + 1).Select(x => (char)x).ToList();
            result.AddRange(Enumerable.Range('a', 'z' - 'a' + 1).Select(x => (char)x).ToList());
            result.Add(' ');
            result.Add('.');
            result.Add(',');
            result.Add(':');

            return result.ToArray();
        }
    }
}
