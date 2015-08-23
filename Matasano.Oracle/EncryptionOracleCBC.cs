using Matasano.Cipher.AES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matasano.Oracle
{
    public class EncryptionOracleCBC
    {
        Random random = new Random();
        AESCipherCBC cipherCBC = new AESCipherCBC();
        AESCipherHelper helper = new AESCipherHelper();
        private string key;

        public EncryptionOracleCBC()
        {
            key = helper.GenerateKey().ToString();
        }

        public Tuple<Base64, string> Encrypt(string data)
        {
            string iv = helper.GenerateKey().ToString();

            return new Tuple<Base64, string>(cipherCBC.Encrypt(key, data, iv), iv);
        }

        public string Decrypt(Hex data, string iv)
        {
            return cipherCBC.Decrypt(key, data, iv);
        }

        public bool IsValidPadding(Hex data, string iv)
        {
            return cipherCBC.IsValidPadding(key, data, iv);
        }
    }
}
