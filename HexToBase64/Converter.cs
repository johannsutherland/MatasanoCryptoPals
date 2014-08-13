using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matasano
{
    public class Converter
    {
        public string HexToBase64(string hex)
        {
            string[] hexNumbers = GetHexNumbers(hex);
            byte[] bytes = new byte[hexNumbers.Count()];
            int n = 0;
            foreach (string h in hexNumbers)
            {
                bytes[n++] = (byte)int.Parse(h, System.Globalization.NumberStyles.HexNumber);
            }
            return Convert.ToBase64String(bytes, Base64FormattingOptions.None);
        }

        private string[] GetHexNumbers(string hex)
        {
            if (hex.Contains('-'))
                return hex.Split('-');

            string[] h = new string[hex.Length / 2];
            int n = 0;
            while (n*2 < hex.Length)
            {
                h[n] = hex.Substring(n*2, 2);
                n++;
            }
            return h;
        }
    }
}
