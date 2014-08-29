using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matasano
{
    public class XorCipher : Cipher
    {
        private readonly Converter converter;

        public XorCipher()
        {
            converter = new Converter();
        }

        public override string Encrypt(string source, string key)
        {
            string createdKey = PadKey(source, key);

            string encrypted = converter.Xor(converter.StringToHex(source), converter.StringToHex(createdKey));

            return encrypted;
        }

        private string PadKey(string source, string key)
        {
            StringBuilder s = new StringBuilder();
            int pos = 0;
            while (pos < source.Length)
            {
                s.Append(key);
                pos += key.Length;
            }

            string createdKey = s.ToString().Substring(0, source.Length);
            return createdKey;
        }

        public override string Decrypt(string hexSource, string key)
        {
            return converter.HexToString(converter.Xor(hexSource, converter.StringToHex(key)));
        }
    }
}
