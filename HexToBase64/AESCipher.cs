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
        public string IsECB(string location)
        {
            int blockSize = 16;

            foreach (string line in File.ReadAllLines(location))
            {
                var blocks = line.SplitByLength(blockSize);
                var groups = blocks.GroupBy(x => x);
                var repeats = groups.Where(x => x.Count() > 1);

                if (repeats.Count() > 0)
                {
                    return line;
                }
            }

            return "";
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
                Padding = PaddingMode.None,
                BlockSize = blockSize
            };
            ICryptoTransform decrypter = aesAlg.CreateDecryptor();

            byte[] message = conv.HexToBytes(conv.Base64ToHex(String.Join("", File.ReadAllLines(location))));
            StringBuilder decoded = new StringBuilder();
            byte[] outputBuffer = new byte[blockSize];

            for (int pos = 0; pos < message.Length / blockSize; pos ++)
            {
                byte[] inputBuffer;

                if (pos * blockSize > message.Length)
                {
                    inputBuffer = message.Skip(pos * blockSize).ToArray();
                    outputBuffer = decrypter.TransformFinalBlock(inputBuffer, 0, message.Length - pos * blockSize);
                }
                else
                {
                    inputBuffer = message.Skip(pos * blockSize).Take(blockSize).ToArray();
                    decrypter.TransformBlock(inputBuffer, 0, blockSize, outputBuffer, 0);
                }

                decoded.Append(conv.HexToString(conv.BytesToHex(outputBuffer)));
            }

            decoded.ToString();
        }
    }
}
