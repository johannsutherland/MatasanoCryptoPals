using Matasano.Cipher.AES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matasano.Oracle
{
    public class CBCEncryptionOracle
    {
        Random random = new Random();
        AESCipherCBC cipherCBC = new AESCipherCBC();
        AESCipherHelper helper = new AESCipherHelper();
        Bytes key;

        public CBCEncryptionOracle()
        {
            key = helper.GenerateKey();
        }

        public Base64 Encrypt(string data, Bytes iv)
        {
            return cipherCBC.Encrypt(key.ToString(), data, iv.ToString());
        }
    }
}
