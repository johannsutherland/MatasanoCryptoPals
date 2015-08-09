using System;

using Matasano.Cipher.AES;

namespace Matasano.Oracle
{
    public class EncryptionOracle : IEncryptionOracle
    {
        Random random = new Random();
        AESCipherECB cipherECB = new AESCipherECB();
        AESCipherCBC cipherCBC = new AESCipherCBC();
        protected AESCipherHelper helper = new AESCipherHelper();
        Bytes key;

        public EncryptionOracle()
        {
            key = helper.GenerateKey();
        }

        public Base64 EncryptConsistentKey(string data, Base64 unknownString, int startIndex = 0)
        {
            string plaintext = data + unknownString.Decode().Substring(startIndex);
            return cipherECB.Encrypt(key.ToString(), plaintext);
        }

        public Tuple<Base64, string> EncryptWithRandomPadding(string data)
        {
            string plainText = PadData(data);

            if (random.Next(2) == 1)
            {
                return new Tuple<Base64, string>(cipherCBC.Encrypt(plainText), "CBC");
            }
            else
            {
                return new Tuple<Base64, string>(cipherECB.Encrypt(plainText), "EBC");
            }
        }

        private string PadData(string data)
        {
            int startSize = 5 + random.Next(5);
            int endSize = 5 + random.Next(5);

            byte[] startPad = new byte[startSize];
            byte[] endPad = new byte[endSize];

            random.NextBytes(startPad);
            random.NextBytes(endPad);

            Bytes result = new Bytes(startPad);
            result.Add(new Bytes(data).ToArray());
            result.Add(endPad);

            return result.ToString();
        }
    }
}
