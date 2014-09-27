﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matasano;
using System.IO;

namespace ConverterTests
{
    [TestClass]
    public class AESTests
    {
        [TestMethod]
        public void EncryptAES()
        {
            string expected = String.Join("", File.ReadAllLines(@"TestFiles\AESEncrypted.txt"));
            string data = File.ReadAllText(@"TestFiles\AESDecrypted.txt");

            AESCipher aes = new AESCipher();
            int lengthWithoutPadding = 3813;
            string result = aes.Encrypt("YELLOW SUBMARINE", data);
            Assert.AreEqual(expected.Substring(0, lengthWithoutPadding), result.Substring(0, lengthWithoutPadding));
        }

        [TestMethod]
        public void EncryptAndDecryptAES()
        {
            string key = "YELLOW SUBMARINE";
            string data = "this is the data";

            AESCipher aes = new AESCipher();
            string encrypted = aes.Encrypt(key, data);
            string decrypted = aes.Decrypt(key, encrypted);

            Assert.AreEqual(data, decrypted);
        }

        [TestMethod]
        public void DecryptAES()
        {
            string expected = File.ReadAllText(@"TestFiles\AESDecrypted.txt");
            string data = File.ReadAllText(@"TestFiles\AESEncrypted.txt");

            AESCipher aes = new AESCipher();
            string result = aes.Decrypt("YELLOW SUBMARINE", data);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DecryptAESCBC()
        {
            string expected = File.ReadAllText(@"TestFiles\AESDecrypted.txt");
            string data = String.Join("", File.ReadAllLines(@"TestFiles\AESCBCEncrypted.txt"));
            string iv = new string('\0', 128);

            AESCipher aes = new AESCipher();
            string result = aes.DecryptCBC("YELLOW SUBMARINE", data, iv);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DetectECB()
        {
            AESCipher aes = new AESCipher();
            string expected = "d880619740a8a19b7840a8a31c810a3d08649af70dc06f4fd5d2d69c744cd283e2dd052f6b641dbf9d11b0348542bb5708649af70dc06f4fd5d2d69c744cd2839475c9dfdbc1d46597949d9c7e82bf5a08649af70dc06f4fd5d2d69c744cd28397a93eab8d6aecd566489154789a6b0308649af70dc06f4fd5d2d69c744cd283d403180c98c8f6db1f2a3f9c4040deb0ab51b29933f2c123c58386b06fba186a";
            string actual = "";

            foreach (string line in File.ReadAllLines(@"TestFiles\AESinECBdetection.txt"))
            {
                if (aes.IsECB(line))
                {
                    actual = line;
                    break;
                }
            }

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PadKey()
        {
            AESCipher aes = new AESCipher();
            string key = "YELLOW SUBMARINE";
            string expected = "YELLOW SUBMARINE\x04\x04\x04\x04";
            string actual = aes.PadKey(key, 20);

            Assert.AreEqual(expected, actual);
        }
    }
}
