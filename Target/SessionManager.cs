using System;
using System.IO;

using Matasano;
using Matasano.Cipher.AES;

namespace Target
{
    public class SessionManager
    {
        AESCipherCBC cipherCBC = new AESCipherCBC();
        AESCipherHelper helper = new AESCipherHelper();
        Random random = new Random();

        internal string decrypted;
        internal string decoded;

        private string key;
        private const string SessionValues = @"Resources\Sessions.txt";

        public SessionManager()
        {
            key = helper.GenerateKey().ToString();
        }

        public Tuple<Hex, string> GetEncryptedCookie()
        {
            string[] lines = File.ReadAllLines(SessionValues);
            string line = lines[random.Next(lines.Length)];

            decoded = new Base64(line).Decode();
            string iv = helper.GenerateKey().ToString();

            return new Tuple<Hex, string>(cipherCBC.Encrypt(key, decoded, iv), iv);
        }

        public void ReceiveEncryptedCookie(Hex data, string iv)
        {
            decrypted = cipherCBC.Decrypt(key, data, iv);
        }
    }
}
