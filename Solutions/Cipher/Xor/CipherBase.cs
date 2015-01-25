using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Matasano.Helper;

namespace Matasano
{
    public abstract class CipherBase
    {
        public abstract string Decrypt(Hex source, string key);
        public abstract string Encrypt(string source, string key);

        private readonly CharacterCounter characterCounter;

        public CipherBase()
        {
            characterCounter = new CharacterCounter();
        }

        public Dictionary<char, string> TryDecrypt(Hex source)
        {
            var result = new Dictionary<char, string>();

            foreach (char c in characterCounter.GetAlphabet())
            {
                string s = new string(c, source.Length);
                string decrypted = this.Decrypt(source, s);

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
