﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matasano
{
    public class EncryptionOracle : IEncryptionOracle
    {
        protected Random random = new Random();
        protected AESCipher cipher = new AESCipher();
        protected AESCipherHelper helper = new AESCipherHelper();
        protected Bytes key;

        public EncryptionOracle()
        {
            key = helper.GenerateKey();
        }

        public Base64 EncryptConsistentKey(string data, Base64 unknownString, int startIndex = 0)
        {
            string plaintext = data + unknownString.Decode().Substring(startIndex);
            return cipher.EncryptECB(key.ToString(), plaintext);
        }

        public Tuple<Base64, string> EncryptWithRandomPadding(string data)
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