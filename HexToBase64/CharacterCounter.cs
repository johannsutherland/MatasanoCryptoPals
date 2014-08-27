using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matasano
{
    public class CharacterManager
    {
        public Dictionary<char, int> Frequency(string sequence)
        {
            var dictionary = new Dictionary<char, int>();
            
            foreach (char s in sequence)
            {
                if (dictionary.ContainsKey(s))
                    dictionary[s]++;
                else
                    dictionary.Add(s, 1);
            }

            return dictionary;
        }

        public List<char> GetAlphabet(bool extend = false)
        {
            var result = Enumerable.Range('A', 'Z' - 'A' + 1).Select(x => (char)x).ToList();
            if (extend)
            {
                result.AddRange(Enumerable.Range('a', 'z' - 'a' + 1).Select(x => (char)x).ToList());
                result.Add(' ');
                result.Add((char)10);
                result.Add('\'');
                result.AddRange(Enumerable.Range('0', '9' - '0' + 1).Select(x => (char)x).ToList());
            }
            return result;
        }

        public char FindKey(Dictionary<char, string> decrypted)
        {
            foreach (KeyValuePair<char, string> key in decrypted)
            {
                var frequency = Frequency(key.Value);
                var kvp = (from entry in frequency orderby entry.Value descending select entry);

                if (kvp.All(x => GetAlphabet(true).Contains(x.Key)))
                {
                    return key.Key;
                }
            }
            return new char();
        }
    }
}
