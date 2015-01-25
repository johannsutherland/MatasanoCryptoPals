using System;
using System.Collections.Generic;
using System.IO;

using Matasano.Helper;
using Matasano.Cipher.Xor;

namespace Matasano.Cracker
{
    public class XorCrackerHelper
    {
        private readonly CharacterCounter characterCounter;
        private readonly XorCipher xorCipher;

        public XorCrackerHelper()
        {
            this.characterCounter = new CharacterCounter();
            this.xorCipher = new XorCipher();
        }

        public Dictionary<char, string> TryDecrypt(Hex source)
        {
            var result = new Dictionary<char, string>();

            foreach (char c in characterCounter.GetAlphabet())
            {
                string s = new string(c, source.Length);
                string decrypted = xorCipher.Decrypt(source, s);

                result.Add(c, decrypted);
            }

            return result;
        }

        public char TryDecryptAndFindKey(Hex source)
        {
            return characterCounter.FindKey(this.TryDecrypt(source));
        }

        public string TryDecryptFile(string location)
        {
            string[] lines = File.ReadAllLines(location);
            foreach (string line in lines)
            {
                var decrypted = this.TryDecrypt(new Hex(line));
                char c = characterCounter.FindKey(decrypted);
                if (c != '\0')
                {
                    return decrypted[c];
                }
            }
            return String.Empty;
        }
    }
}
