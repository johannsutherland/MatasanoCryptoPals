using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matasano.Cipher.AES;
using System.IO;
using Attacker.Cipher;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Target;
using Attacker.Cracker.AES;

namespace Matasano.Cipher.AES.Tests
{
    [TestClass]
    public class AESCipherCTRTests
    {
        const string key = "YELLOW SUBMARINE";

        [TestMethod]
        public void GetFirstNonceTest()
        {
            AESCipherCTR cipher = new AESCipherCTR(key, 0);
            Bytes expected = new Bytes(new byte[16]);

            string nonce = cipher.GetNextCounter();

            Assert.AreEqual(expected.ToString(), nonce);
        }

        [TestMethod]
        public void GetNextNonceTest()
        {
            AESCipherCTR cipher = new AESCipherCTR(key, 0);
            Bytes expected = new Bytes(new byte[16]);
            expected[8] = 1;

            string nonce = cipher.GetNextCounter();
            string nextNonce = cipher.GetNextCounter();

            Assert.AreEqual(expected.ToString(), nextNonce);
        }

        [TestMethod]
        [TestCategory("Set 3 - Challenge 18")]
        public void DecryptAESCTR()
        {
            AESCipherCTR cipher = new AESCipherCTR(key, 0);
            Base64 data = new Base64("L77na/nrFsKvynd6HzOoG7GHTLXsTVu9qvY/2syLXzhPweyyMTJULu/6/kXX0KSvoOLSFQ==");
            string expected = "Yo, VIP Let's kick it Ice, Ice, baby Ice, Ice, baby ";

            string decrypted = cipher.Decrypt(data.Decode());

            Assert.AreEqual(expected, decrypted);
        }

        [TestMethod]
        public void EncryptAESCTR()
        {
            AESCipherCTR cipher = new AESCipherCTR(key, 0);
            Base64 expected = new Base64("L77na/nrFsKvynd6HzOoG7GHTLXsTVu9qvY/2syLXzhPweyyMTJULu/6/kXX0KSvoOLSFQ==");
            string data = "Yo, VIP Let's kick it Ice, Ice, baby Ice, Ice, baby ";

            string encrypted = cipher.Encrypt(data);

            Assert.AreEqual(expected.Decode(), encrypted);
        }

        [TestMethod]
        [TestCategory("Set 3 - Challenge 19")]
        public void CrackAESCTR()
        {
            var encryptedLines = new AESFixedNonceEncryptor().EncryptFile(@"TestFiles\AESCTR.txt");
            var crackedLines = new AESFixedNonceCracker().Crack(encryptedLines);

            var decryptedLines = File.ReadAllLines(@"TestFiles\AESCTRDecrypted.txt");

            Assert.AreEqual(crackedLines.Count, decryptedLines.Count());
            for (int i = 0; i < crackedLines.Count; i++)
            {
                Assert.AreEqual(crackedLines[i], decryptedLines[i], String.Format("Line {0} differs. {1} doesn't equal {2}"
                    , i, crackedLines[i], decryptedLines[i]));
            }
        }
    }
}
