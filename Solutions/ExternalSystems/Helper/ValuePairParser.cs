using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matasano.Helper
{
    public class ValuePairParser
    {
        private Dictionary<string, string> values = new Dictionary<string, string>();
        private char splitCharacter;

        public ValuePairParser(string cookie, char splitCharacter = '&')
        {
            this.splitCharacter = splitCharacter;

            cookie.Split(splitCharacter).ToList().ForEach(token =>
            {
                var kvp = token.Split('=');
                if (kvp.Length == 2) values.Add(kvp[0], kvp[1]);
            });
        }

        public string this[string key]
        {
            get { return values[key]; }
        }

        public bool Contains(string key)
        {
            return values.ContainsKey(key);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            values.ToList().ForEach(kvp => sb.Append(kvp.Key + "=" + kvp.Value + splitCharacter.ToString()));
            string result = sb.ToString();
            return result.Substring(0, result.Length - 1);
        }
    }
}
