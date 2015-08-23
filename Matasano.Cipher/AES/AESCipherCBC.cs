using System;
using System.Linq;
using System.Security.Cryptography;

namespace Matasano.Cipher.AES
{
    public class AESCipherCBC : AESCipher
    {
        public string Decrypt(string key, Hex data, string iv)
        {
            byte[] convertedIV = new Bytes(helper.AddPadding(iv)).ToArray();

            byte[] message = ((Bytes)data).ToArray();
            byte[] xor = new byte[message.Length];

            convertedIV.CopyTo(xor, 0);
            message.Take(message.Length - blockSize).ToArray().CopyTo(xor, blockSize);

            Bytes outputBuffer = DecryptToBytes(key, data);

            string decoded = outputBuffer.Xor(new Bytes(xor)).ToString();

            return helper.RemovePadding(decoded);
        }

        public bool IsValidPadding(string key, Hex data, string iv)
        {
            bool isValid = false;

            try
            {
                var decrypted = Decrypt(key, data, iv);
                isValid = true;
            }
            catch (InvalidPaddingException)
            {
                isValid = false;
            }

            return isValid;
        }

        public Base64 Encrypt(string data)
        {
            Bytes key = helper.GenerateKey();
            Bytes iv = new Bytes(new String('\0', 16));
            return this.Encrypt(key, data, iv);
        }

        public Base64 Encrypt(string key, string data, string iv)
        {
            return this.Encrypt(new Bytes(key), data, new Bytes(iv));
        }

        private Base64 Encrypt(Bytes key, string data, Bytes iv)
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
            Bytes encrypted = new Bytes(String.Empty);
            byte[] outputBuffer = iv.ToArray();

            for (int pos = 0; pos < message.Length / blockSize; pos++)
            {
                Bytes inputBuffer = new Bytes(message.Skip(pos * blockSize).Take(blockSize).ToArray()).Xor(new Bytes(outputBuffer));
                encryptor.TransformBlock(inputBuffer.ToArray(), 0, blockSize, outputBuffer, 0);

                encrypted.Add(outputBuffer);
            }

            return encrypted;
        }
    }
}
