using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matasano.Cipher.AES;

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

            Bytes nonce = new Bytes(cipher.GetNextCounter());

            Assert.AreEqual(expected.ToString(), nonce.ToString());
        }

        [TestMethod]
        public void GetNextNonceTest()
        {
            AESCipherCTR cipher = new AESCipherCTR(key, 0);
            Bytes expected = new Bytes(new byte[16]);
            expected[8] = 1;

            Bytes nonce = new Bytes(cipher.GetNextCounter());
            Bytes nextNonce = new Bytes(cipher.GetNextCounter());

            Assert.AreEqual(expected.ToString(), nextNonce.ToString());
        }
    }
}
