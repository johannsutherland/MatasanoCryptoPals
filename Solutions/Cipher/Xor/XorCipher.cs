using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matasano.Cipher.Xor
{
    public class XorCipher : CipherBase
    {
        public override string Encrypt(string source, string key)
        {
            string createdKey = PadKey(source, key);

            Hex encrypted = new Hex(source, Hex.InputFormat.String).Xor(new Hex(createdKey, Hex.InputFormat.String));

            return encrypted.ToString();
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

        public override string Decrypt(Hex source, string key)
        {
            Hex decrypted = source.Xor(new Hex(key, Hex.InputFormat.String));
            return decrypted.ToString();
        }
    }
}
