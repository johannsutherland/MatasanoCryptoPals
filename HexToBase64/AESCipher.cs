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

        public string Decrypt(string key, string data)
        {
            Converter conv = new Converter();

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
            StringBuilder decoded = new StringBuilder();
            byte[] outputBuffer = new byte[message.Length];

            decrypter.TransformBlock(message, 0, message.Length, outputBuffer, 0);
            decoded.Append(conv.HexToString(conv.BytesToHex(outputBuffer)));

            return decoded.ToString().Trim('\0');
        }

        public string Encrypt(string key, string data)
        {
            Converter conv = new Converter();

            byte[] convertedKey = conv.HexToBytes(conv.StringToHex(key));
            int blockSize = 128;

            RijndaelManaged aesAlg = new RijndaelManaged
            {
                Key = convertedKey,
                Mode = CipherMode.ECB,
                BlockSize = blockSize
            };
            ICryptoTransform encryptor = aesAlg.CreateEncryptor();

            byte[] message = conv.HexToBytes(conv.StringToHex(PadKey(data, blockSize)));
            StringBuilder encoded = new StringBuilder();
            byte[] outputBuffer = new byte[message.Length];

            encryptor.TransformBlock(message, 0, message.Length, outputBuffer, 0);
            encoded.Append(conv.HexToBase64(conv.BytesToHex(outputBuffer)));

            return encoded.ToString();
        }
    }
}
