using System;
using System.Linq;
using System.Security.Cryptography;

namespace Matasano.Cipher.AES
{
    public abstract class AESCipher
    {
        protected int blockSizeBits;
        protected AESCipherHelper helper;
        protected int blockSize;

        public AESCipher(int blockSize = 16)
        {
            this.blockSizeBits = blockSize * 8;
            this.blockSize = blockSize;
            helper = new AESCipherHelper(blockSize);
        }

        protected Bytes DecryptToBytes(string key, Hex data)
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
    }
}
