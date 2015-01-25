using System;
using System.Linq;
using System.Security.Cryptography;

namespace Matasano.Cipher.AES
{
    public class AESCipher
    {
        private int blockSizeBits;
        private int blockSize;
        private AESCipherHelper helper;

        public AESCipher(int blockSize = 16)
        {
            this.blockSizeBits = blockSize * 8;
            this.blockSize = blockSize;
            helper = new AESCipherHelper(blockSize);
        }

        private Bytes DecryptECBToBytes(string key, Hex data)
        {
            byte[] convertedKey = new Bytes(key).ToArray();

            RijndaelManaged aesAlg = new RijndaelManaged
            {
                Key = convertedKey,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.None,
                BlockSize = blockSizeBits
            };
            ICryptoTransform decrypter = aesAlg.CreateDecryptor();

            byte[] message = ((Bytes)data).ToArray();
            byte[] outputBuffer = new byte[message.Length];

            decrypter.TransformBlock(message, 0, message.Length, outputBuffer, 0);

            return new Bytes(outputBuffer);
        }

        public string DecryptECB(string key, string data)
        {
            return this.DecryptECB(key, new Hex(data, Hex.InputFormat.String));
        }

        public string DecryptECB(string key, Hex data)
        {
            Bytes outputBuffer = DecryptECBToBytes(key, data);

            string decoded = outputBuffer.ToString();

            return helper.RemovePadding(decoded);
        }

        public string DecryptCBC(string key, Hex data, string iv)
        {
            byte[] convertedIV = new Bytes(helper.AddPadding(iv)).ToArray();

            byte[] message = ((Bytes)data).ToArray();
            byte[] xor = new byte[message.Length];

            convertedIV.CopyTo(xor, 0);
            message.Take(message.Length - blockSize).ToArray().CopyTo(xor, blockSize);

            Bytes outputBuffer = DecryptECBToBytes(key, data);
            
            string decoded = outputBuffer.Xor(new Bytes(xor)).ToString();

            return helper.RemovePadding(decoded);
        }

        public Base64 EncryptECB(string data)
        {
            Bytes key = helper.GenerateKey();
            return this.EncryptECB(key, data);
        }

        public Base64 EncryptECB(string key, string data)
        {
            return this.EncryptECB(new Bytes(key), data);
        }

        private Base64 EncryptECB(Bytes key, string data)
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

        public Base64 EncryptCBC(string data)
        {
            Bytes key = helper.GenerateKey();
            Bytes iv = new Bytes(new String('\0', 16));
            return this.EncryptCBC(key, data, iv);
        }

        public Base64 EncryptCBC(string key, string data, string iv)
        {
            return this.EncryptCBC(new Bytes(key), data, new Bytes(iv));
        }

        private Base64 EncryptCBC(Bytes key, string data, Bytes iv)
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
