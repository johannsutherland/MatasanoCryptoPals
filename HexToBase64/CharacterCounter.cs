using System;
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

        public List<char> GetAlphabet()
        {
            var result = Enumerable.Range('A', 'Z' - 'A' + 1).Select(x => (char)x).ToList();
            result.AddRange(Enumerable.Range('a', 'z' - 'a' + 1).Select(x => (char)x).ToList());
            result.AddRange(Enumerable.Range('0', '9' - '0' + 1).Select(x => (char)x).ToList());
            result.Add((char)9);
            result.Add((char)10);
            result.Add(',');
            result.Add('.');
            result.Add(' ');
            result.Add('?');
            result.Add('!');
            result.Add('[');
            result.Add(']');
            result.Add(':');
            result.Add('\'');
            result.Add('\0');

            return result;
        }

        public char FindKey(Dictionary<char, string> decrypted)
        {
            var result = new Dictionary<char, float>();

            foreach (KeyValuePair<char, string> key in decrypted)
            {
                var frequency = Frequency(key.Value);
                var grouped = frequency.GroupBy(x => GetAlphabet().Contains(x.Key) ? "Valid" : "Invalid");
                var dictionary = grouped.ToDictionary(x => x.Key);

                float valid = GetSumOfValue(dictionary, "Valid");
                float invalid = GetSumOfValue(dictionary, "Invalid");
                float ratio = CalculateRatio(valid, invalid);

                result.Add(key.Key, ratio);
            }
            var bestMatch = result.OrderByDescending(x => x.Value).Take(1).Single();

            if (bestMatch.Value > 0.9)
                return bestMatch.Key;
            else
                return new char();
        }

        private static float GetSumOfValue(Dictionary<string, IGrouping<string, KeyValuePair<char, int>>> dictionary, string key)
        {
            float invalid = 0;
            if (dictionary.ContainsKey(key))
                invalid = dictionary[key].ToList().Sum(x => (float)x.Value);
            return invalid;
        }

        private static float CalculateRatio(float valid, float invalid)
        {
            float ratio = 0;

            if (valid == 0)
                ratio = 0;
            else if (invalid == 0)
                ratio = 1;
            else
                ratio = valid / (valid + invalid);
            return ratio;
        }
    }
}
