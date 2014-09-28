using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matasano
{
    public class EncryptionOracle
    {
        Random random = new Random();
        AESCipher cipher = new AESCipher();

        public Tuple<Base64, string> Encrypt(string data)
        {
            string plainText = PadData(data);

            if (random.Next(2) == 1)
            {
                return new Tuple<Base64, string>(cipher.EncryptCBC(plainText), "CBC");
            }
            else
            {
                return new Tuple<Base64, string>(cipher.EncryptECB(plainText), "EBC");
            }
        }

        public Base64 EncryptConsistentKey(string data)
        {
            
        }

        private string PadData(string data)
        {
            int startSize = 5 + random.Next(5);
            int endSize = 5 + random.Next(5);

            byte[] startPad = new byte[startSize];
            byte[] endPad = new byte[endSize];

            random.NextBytes(startPad);
            random.NextBytes(endPad);

            Bytes result = new Bytes(startPad);
            result.Add(new Bytes(data).ToArray());
            result.Add(endPad);

            return result.ToString();
        }
    }
}
