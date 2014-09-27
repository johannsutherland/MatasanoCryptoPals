using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Matasano
{
    public class AESCipher
    {
        private int blockSize;

        public AESCipher(int blockSize = 128)
        {
            this.blockSize = blockSize;
        }

        public bool IsECB(string line)
        {
            var blocks = line.SplitByLength(blockSize);
            var groups = blocks.GroupBy(x => x);
            var repeats = groups.Where(x => x.Count() > 1);

            return (repeats.Count() > 0);
        }

        public string AddPadding(string data)
        {
            if (data.Length % blockSize == 0)
            {
                return data;
            }
            else
            {
                int paddingLength = blockSize - (data.Length % blockSize);
                return data + new String((char)(paddingLength), paddingLength);
            }
        }

        public string RemovePadding(string data)
        {
            string cleanedData = data.Trim('\0');
            char c = cleanedData[cleanedData.Length - 1];
            int padding = (int)c;

            if (data.Substring(data.Length - padding).All(x => x == c || x == '\0'))
                return cleanedData.Substring(0, data.Length - padding);
            else
                return cleanedData;
        }

        private Bytes DecryptToBytes(string key, Hex data)
        {
            byte[] convertedKey = new Bytes(key).ToArray();

            RijndaelManaged aesAlg = new RijndaelManaged
            {
                Key = convertedKey,
                Mode = CipherMode.ECB,
                BlockSize = blockSize
            };
            ICryptoTransform decrypter = aesAlg.CreateDecryptor();

            byte[] message = data.ToBytes().ToArray();
            byte[] outputBuffer = new byte[message.Length];

            decrypter.TransformBlock(message, 0, message.Length, outputBuffer, 0);

            return new Bytes(outputBuffer);
        }

        public string Decrypt(string key, Base64 data)
        {
            return this.Decrypt(key, data.ToHex());
        }

        public string Decrypt(string key, string data)
        {
            return this.Decrypt(key, new Hex(data, Hex.InputFormat.String));
        }

        public string Decrypt(string key, Hex data)
        {
            Bytes outputBuffer = DecryptToBytes(key, data);

            string decoded = outputBuffer.ToString();

            return RemovePadding(decoded);
        }

        public string DecryptCBC(string key, Hex data, string iv)
        {
            byte[] convertedIV = new Bytes(AddPadding(iv)).ToArray();

            byte[] message = data.ToBytes().ToArray();
            byte[] xor = new byte[message.Length];

            convertedIV.CopyTo(xor, 0);
            message.Take(message.Length - blockSize / 8).ToArray().CopyTo(xor, blockSize / 8);

            Bytes outputBuffer = DecryptToBytes(key, data);
            
            int length = outputBuffer.ToString().TrimEnd(new char[] { '\0' }).Length;
            string decoded = outputBuffer.Xor(new Bytes(xor)).ToString();

            return decoded.Substring(0, length);
        }

        public Base64 Encrypt(string key, string data)
        {
            byte[] convertedKey = new Bytes(key).ToArray();

            RijndaelManaged aesAlg = new RijndaelManaged
            {
                Key = convertedKey,
                Mode = CipherMode.ECB,
                BlockSize = blockSize
            };
            ICryptoTransform encryptor = aesAlg.CreateEncryptor();

            string paddedData = AddPadding(data);
            byte[] message = new Bytes(paddedData).ToArray();
            byte[] outputBuffer = new byte[message.Length];

            encryptor.TransformBlock(message, 0, message.Length, outputBuffer, 0);

            return new Bytes(outputBuffer).ToBase64();
        }
    }
}
