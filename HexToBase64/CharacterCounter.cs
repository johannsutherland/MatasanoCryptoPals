using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matasano
{
    public class CharacterCounter
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

        public Dictionary<char, string> Decrypt(string hexSource)
        {
            Converter conv = new Converter();
            var result = new Dictionary<char, string>();

            foreach (char c in GetAlphabet(true))
            {
                string s = new string(c, hexSource.Length / 2);
                string decrypted = conv.HexToString(conv.Xor(hexSource, conv.StringToHex(s)));

                result.Add(c, decrypted);
            }

            return result;
        }

        public string Encrypt(string source, string key)
        {
            Converter conv = new Converter();

            StringBuilder s = new StringBuilder();
            int pos = 0;
            while (pos < source.Length)
            {
                s.Append(key);
                pos += key.Length;
            }

            string createdKey = s.ToString().Substring(0, source.Length);

            string encrypted = conv.Xor(conv.StringToHex(source), conv.StringToHex(createdKey));

            return encrypted;
        }

        public string DecryptFile(string location)
        {
            string[] lines = File.ReadAllLines(location);
            foreach (string line in lines)
            {
                var decrypted = this.Decrypt(line);
                if (line == "7b5a4215415d544115415d5015455447414c155c46155f4058455c5b523f")
                {
                    Debug.WriteLine("found it");
                }
                char c = this.FindKey(decrypted);
                if (c != '\0')
                {
                    return decrypted[c];
                }
            }
            return String.Empty;
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

        public int HammingDistance(string str1, string str2)
        {
            if (str1.Length != str2.Length)
                throw new ArgumentException("String lengths must be equal");

            var bytes1 = new BitArray(Encoding.Unicode.GetBytes(str1.ToCharArray()));
            var bytes2 = new BitArray(Encoding.Unicode.GetBytes(str2.ToCharArray()));

            var results = bytes1.Xor(bytes2);

            int difference = 0;

            foreach (bool bit in results)
            {
                difference += bit ? 1 : 0;
            }

            return difference;
        }
    }
}
