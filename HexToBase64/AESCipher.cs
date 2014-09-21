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

        public string PadKey(string key, int length)
        {
            if (key.Length >= length)
            {
                return key;
            }
            else
            {
                int paddingLength = length - key.Length;
                return key + new String((char)(paddingLength), paddingLength);
            }
        }

        public void Decrypt(string key, string location)
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

            byte[] message = conv.HexToBytes(conv.Base64ToHex(String.Join("", File.ReadAllLines(location))));
            StringBuilder decoded = new StringBuilder();
            byte[] outputBuffer = new byte[message.Length];

            decrypter.TransformBlock(message, 0, message.Length, outputBuffer, 0);
            decoded.Append(conv.HexToString(conv.BytesToHex(outputBuffer)));

            decoded.ToString();
        }
    }
}
