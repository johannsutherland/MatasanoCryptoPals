using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matasano
{
    public class Converter
    {
        public byte[] HexToBytes(string hex)
        {
            byte[] h = new byte[hex.Length / 2];
            int n = 0;

            while (n * 2 + 2 <= hex.Length)
            {
                h[n] = (byte)int.Parse(hex.Substring(n * 2, 2), System.Globalization.NumberStyles.HexNumber);
                n++;
            }

            return h;
        }

        public string HexToString(string hex)
        {
            StringBuilder result = new StringBuilder();
            int n = 0;

            while (n * 2 + 2 <= hex.Length)
            {
                result.Append((char)int.Parse(hex.Substring(n * 2, 2), System.Globalization.NumberStyles.HexNumber));
                n++;
            }

            return result.ToString();
        }

        public string StringToHex(string s)
        {
            StringBuilder result = new StringBuilder();

            foreach (char c in s)
            {
                result.Append(((byte)c).ToString("x2"));
            }

            return result.ToString();
        }

        public string BytesToHex(byte[] result)
        {
            StringBuilder sb = new StringBuilder();
            result.ToList().ForEach(x => sb.Append(x.ToString("x2")));

            return sb.ToString();
        }

        public string HexToBase64(string hex)
        {
            byte[] bytes = HexToBytes(hex);
            return Convert.ToBase64String(bytes, Base64FormattingOptions.None);
        }

        public string Base64ToHex(string base64)
        {
            return this.BytesToHex(Convert.FromBase64String(base64));
        }

        public byte[] Xor(byte[] op1, byte[] op2)
        {
            if (op1.Length != op2.Length)
            {
                throw new ArgumentException("Lengths must be equal");
            }

            byte[] result = new byte[op1.Length];
            for (int i = 0; i < op1.Length; i++)
            {
                result[i] = (byte)(op1[i] ^ op2[i]);
            }

            return result;
        }

        public string Xor(string op1, string op2)
        {
            byte[] result = Xor(HexToBytes(op1), HexToBytes(op2));
            return BytesToHex(result);
        }
    }

    public static class StringExtensions
    {
        public static IEnumerable<string> SplitByLength(this string s, int length)
        {
            for (int i = 0; i < s.Length; i += length)
            {
                if (i + length <= s.Length)
                {
                    yield return s.Substring(i, length);
                }
                else
                {
                    yield return s.Substring(i);
                }
            }
        }
    }
}
