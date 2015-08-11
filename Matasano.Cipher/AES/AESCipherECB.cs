using System.Security.Cryptography;

namespace Matasano.Cipher.AES
{
    public class AESCipherECB : AESCipher
    {
        public string Decrypt(string key, string data)
        {
            return this.Decrypt(key, new Hex(data, Hex.InputFormat.String));
        }

        public string Decrypt(string key, Hex data)
        {
            Bytes outputBuffer = DecryptToBytes(key, data);
            string decoded = outputBuffer.ToString();
            return helper.RemovePadding(decoded);
        }

        public Base64 Encrypt(string data)
        {
            Bytes key = helper.GenerateKey();
            return this.Encrypt(key, data);
        }

        public Base64 Encrypt(string key, string data)
        {
            return this.Encrypt(new Bytes(key), data);
        }

        private Base64 Encrypt(Bytes key, string data)
        {
            RijndaelManaged aesAlg = new RijndaelManaged
            {
                Key = key.ToArray(),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.None,
                BlockSize = blockSizeBits
            };
            ICryptoTransform encryptor = aesAlg.CreateEncryptor();

            string paddedData = helper.AddPadding(data);
            byte[] message = new Bytes(paddedData).ToArray();
            byte[] outputBuffer = new byte[message.Length];

            encryptor.TransformBlock(message, 0, message.Length, outputBuffer, 0);

            return new Bytes(outputBuffer);
        }
    }
}
