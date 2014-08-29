using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Matasano
{
    public abstract class Cipher
    {
        public abstract string Decrypt(string hexSource, string key);
        public abstract string Encrypt(string source, string key);

        private readonly CharacterCounter characterCounter;

        public Cipher()
        {
            characterCounter = new CharacterCounter();
        }

        public Dictionary<char, string> TryDecrypt(string hexSource)
        {
            var result = new Dictionary<char, string>();

            foreach (char c in characterCounter.GetAlphabet())
            {
                string s = new string(c, hexSource.Length / 2);
                string decrypted = this.Decrypt(hexSource, s);

                result.Add(c, decrypted);
            }

            return result;
        }

        public char TryDecryptAndFindKey(string hexSource)
        {
            return characterCounter.FindKey(this.TryDecrypt(hexSource));
        }

        public string TryDecryptFile(string location)
        {
            string[] lines = File.ReadAllLines(location);
            foreach (string line in lines)
            {
                var decrypted = this.TryDecrypt(line);
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
