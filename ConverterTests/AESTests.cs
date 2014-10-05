using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matasano;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ConverterTests
{
    [TestClass]
    public class AESTests
    {
        private const int blockSize = 16;

        [TestMethod]
        public void EncryptECB()
        {
            Base64 expected = new Base64(new FileInfo(@"TestFiles\AESEncrypted.txt"));
            string data = File.ReadAllText(@"TestFiles\AESDecrypted.txt");

            AESCipher aes = new AESCipher();
            Base64 result = aes.EncryptECB("YELLOW SUBMARINE", data);

            Assert.AreEqual(expected.ToString(), result.ToString());
        }

        [TestMethod]
        public void EncryptAndDecryptECB()
        {
            Random r = new Random();
            byte[] bytes = new byte[r.Next(100)];
            r.NextBytes(bytes);

            string data = new Bytes(bytes).ToString();
            string key = "YELLOW SUBMARINE";

            AESCipher aes = new AESCipher();
            Base64 encrypted = aes.EncryptECB(key, data);
            string decrypted = aes.DecryptECB(key, encrypted);

            Assert.AreEqual(data, decrypted);
        }

        [TestMethod]
        [TestCategory("Set 1 - Challenge 07")]
        public void DecryptECB()
        {
            string expected = File.ReadAllText(@"TestFiles\AESDecrypted.txt");
            Base64 data = new Base64(new FileInfo(@"TestFiles\AESEncrypted.txt"));

            AESCipher aes = new AESCipher();
            string result = aes.DecryptECB("YELLOW SUBMARINE", data);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void EncryptAndDecryptCBC()
        {
            Random r = new Random();
            byte[] bytes = new byte[r.Next(100)];
            r.NextBytes(bytes);

            string data = new Bytes(bytes).ToString();
            string key = "YELLOW SUBMARINE";

            string iv = new String('\0', blockSize);

            AESCipher aes = new AESCipher();
            Base64 encrypted = aes.EncryptCBC(key, data, iv);
            string decrypted = aes.DecryptCBC(key, encrypted, iv);

            Assert.AreEqual(data, decrypted);
        }

        [TestMethod]
        [TestCategory("Set 2 - Challenge 10")]
        public void DecryptCBC()
        {
            string expected = File.ReadAllText(@"TestFiles\AESDecrypted.txt");
            Base64 data = new Base64(new FileInfo(@"TestFiles\AESCBCEncrypted.txt"));
            string iv = new string('\0', blockSize);

            AESCipher aes = new AESCipher();
            string result = aes.DecryptCBC("YELLOW SUBMARINE", data.ToHex(), iv);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [TestCategory("Set 1 - Challenge 08")]
        public void DetectECB()
        {
            AESCipherHelper helper = new AESCipherHelper(blockSize);
            string expected = "d880619740a8a19b7840a8a31c810a3d08649af70dc06f4fd5d2d69c744cd283e2dd052f6b641dbf9d11b0348542bb5708649af70dc06f4fd5d2d69c744cd2839475c9dfdbc1d46597949d9c7e82bf5a08649af70dc06f4fd5d2d69c744cd28397a93eab8d6aecd566489154789a6b0308649af70dc06f4fd5d2d69c744cd283d403180c98c8f6db1f2a3f9c4040deb0ab51b29933f2c123c58386b06fba186a";
            string actual = "";

            foreach (string line in File.ReadAllLines(@"TestFiles\AESinECBdetection.txt"))
            {
                if (helper.IsECB(line))
                {
                    actual = line;
                    break;
                }
            }

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Set 2 - Challenge 09")]
        public void PadKey()
        {
            AESCipherHelper helper = new AESCipherHelper(20);
            string key = "YELLOW SUBMARINE";
            string expected = "YELLOW SUBMARINE\x04\x04\x04\x04";
            string actual = helper.AddPadding(key);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Set 2 - Challenge 15")]
        public void UnPadKey()
        {
            AESCipherHelper helper = new AESCipherHelper(20);
            string key = "YELLOW SUBMARINE\x04\x04\x04\x04";
            string expected = "YELLOW SUBMARINE";
            string actual = helper.RemovePadding(key);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Set 2 - Challenge 15")]
        public void UnPadInvalidKeys1()
        {
            AESCipherHelper helper = new AESCipherHelper(20);
            string key = "YELLOW SUBMARINE\x05\x05\x05\x05";
            string expected = "Invalid Padding";

            try
            {
                string actual = helper.RemovePadding(key);
                Assert.Fail("Exception not thrown");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(expected, ex.Message);
            }
        }

        [TestMethod]
        [TestCategory("Set 2 - Challenge 15")]
        public void UnPadInvalidKeys2()
        {
            AESCipherHelper helper = new AESCipherHelper(20);
            string key = "YELLOW SUBMARINE\x01\x02\x03\x04";
            string expected = "Invalid Padding";

            try
            {
                string actual = helper.RemovePadding(key);
                Assert.Fail("Exception not thrown");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(expected, ex.Message);
            }
        }

        [TestMethod]
        public void GenerateKeyWithCorrectLength()
        {
            AESCipherHelper helper = new AESCipherHelper(blockSize);
            Bytes key = helper.GenerateKey();
            Assert.IsTrue(key.ToHex().Length == blockSize);
        }

        [TestMethod]
        public void GenerateUniqueKeys()
        {
            AESCipherHelper helper = new AESCipherHelper(16);
            List<Bytes> keys = new List<Bytes>();

            while (keys.Count < 100)
            {
                Bytes key = helper.GenerateKey();
                if (!keys.Contains(key))
                {
                    keys.Add(key);
                }
                else
                {
                    throw new Exception("Duplicate key");
                }
            }

            Assert.IsTrue(keys.Count == 100);
        }

        [TestMethod]
        public void Oracle()
        {
            EncryptionOracle eo = new EncryptionOracle();
            AESCipherHelper helper = new AESCipherHelper();

            for (int i = 0; i < 100; i++)
            {
                Random r = new Random();
                Tuple<Base64, string> encrypted = eo.Encrypt(new String((char)r.Next(255), 100));
                Assert.IsTrue((helper.IsECB(encrypted.Item1.Decode()) && encrypted.Item2 == "EBC") || (encrypted.Item2 == "CBC"));
            }
        }
    }
}
