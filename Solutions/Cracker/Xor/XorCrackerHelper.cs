using System;
using System.Collections.Generic;
using System.IO;

using Matasano.Helper;
using Matasano.Cipher.Xor;

namespace Matasano.Cracker
{
    class XorCrackerHelper
    {
        private readonly XorCipher xorCipher;

        public XorCrackerHelper()
        {
            this.xorCipher = new XorCipher();
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
