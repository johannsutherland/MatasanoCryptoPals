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
        private Converter conv = new Converter();
        private const int BlockSize = 128;

        public bool IsECB(string line)
        {
            int blockSize = 16;

            var blocks = line.SplitByLength(blockSize);
            var groups = blocks.GroupBy(x => x);
            var repeats = groups.Where(x => x.Count() > 1);

            return (repeats.Count() > 0);
        }

        public string PadKey(string data, int blockSize)
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

        private byte[] DecryptToBytes(string key, string data)
        {
            byte[] convertedKey = conv.HexToBytes(conv.StringToHex(key));
            int blockSize = 128;

            RijndaelManaged aesAlg = new RijndaelManaged
            {
                Key = convertedKey,
                Mode = CipherMode.ECB,
                BlockSize = blockSize
            };
            ICryptoTransform decrypter = aesAlg.CreateDecryptor();

            byte[] message = conv.HexToBytes(conv.Base64ToHex(data));
            byte[] outputBuffer = new byte[message.Length];

            decrypter.TransformBlock(message, 0, message.Length, outputBuffer, 0);

            return outputBuffer;
        }

        public string Decrypt(string key, string data)
        {
            byte[] outputBuffer = DecryptToBytes(key, data);

            string decoded = conv.HexToString(conv.BytesToHex(outputBuffer));

            return RemovePadding(decoded);
        }

        public string DecryptCBC(string key, string data, string iv)
        {
            byte[] convertedIV = conv.HexToBytes(conv.StringToHex(PadKey(iv, BlockSize)));

            byte[] message = conv.HexToBytes(conv.Base64ToHex(data));
            byte[] xor = new byte[message.Length];

            byte[] outputBuffer = DecryptToBytes(key, data);

            convertedIV.CopyTo(xor, 0);
            message.Take(message.Length - BlockSize / 8).ToArray().CopyTo(xor, BlockSize / 8);
            outputBuffer = DecryptToBytes(key, data);
            
            int length = conv.HexToString(conv.BytesToHex(outputBuffer)).TrimEnd(new char[] { '\0' }).Length;
            string decoded = conv.HexToString(conv.BytesToHex(conv.Xor(outputBuffer, xor)));

            return decoded.Substring(0, length);
        }

        public string Encrypt(string key, string data)
        {
            byte[] convertedKey = conv.HexToBytes(conv.StringToHex(key));
            int blockSize = 128;

            RijndaelManaged aesAlg = new RijndaelManaged
            {
                Key = convertedKey,
                Mode = CipherMode.ECB,
                BlockSize = blockSize
            };
            ICryptoTransform encryptor = aesAlg.CreateEncryptor();

            string paddedData = PadKey(data, blockSize);
            byte[] message = conv.HexToBytes(conv.StringToHex(paddedData));
            byte[] outputBuffer = new byte[message.Length];

            encryptor.TransformBlock(message, 0, message.Length, outputBuffer, 0);

            return conv.HexToBase64(conv.BytesToHex(outputBuffer)));
        }
    }
}
