using System;
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

            while (n * 2 < hex.Length)
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
            int n = 0;

            foreach (char c in s)
            {
                result.Append(((byte)c).ToString("x"));
                n++;
            }

            return result.ToString();
        }

        private string BytesToHex(byte[] result, StringBuilder sb)
        {
            result.ToList().ForEach(x => sb.Append(x.ToString("x")));
            return sb.ToString();
        }

        public string HexToBase64(string hex)
        {
            byte[] bytes = HexToBytes(hex);
            return Convert.ToBase64String(bytes, Base64FormattingOptions.None);
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
            StringBuilder sb = new StringBuilder();

            return BytesToHex(result, sb);
        }
    }
}
