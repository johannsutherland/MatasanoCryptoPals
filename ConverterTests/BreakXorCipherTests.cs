﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matasano;
using System.Diagnostics;

namespace ConverterTests
{
    [TestClass]
    public class BreakXorCipherTests
    {
        [TestMethod]
        public void Decrypt()
        {
            XorCipher cipher = new XorCipher();
            string source = "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736";
            string expected = "Cooking MC's like a pound of bacon";
            char expectedChar = 'X';
            var result = cipher.TryDecrypt(source);
            Assert.AreEqual(expected, result[expectedChar]);
        }

        [TestMethod]
        public void DecryptAndFindKey()
        {
            XorCipher cipher = new XorCipher();
            string source = "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736";
            char expected = 'X';
            var key = cipher.TryDecryptAndFindKey(source);
            Assert.AreEqual(expected, key);
        }

        [TestMethod]
        public void DecryptFile()
        {
            string expected = "Now that the party is jumping" + (char)10;
            XorCipher cipher = new XorCipher();
            string result = cipher.TryDecryptFile("encrypted.txt");
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void EncryptLine()
        {
            XorCipher cipher = new XorCipher();
            string source = "Burning 'em, if you ain't quick and nimble" + (char)10 + "I go crazy when I hear a cymbal";
            string key = "ICE";
            string expected = "0b3637272a2b2e63622c2e69692a23693a2a3c6324202d623d63343c2a26226324272765272a282b2f20430a652e2c652a3124333a653e2b2027630c692b20283165286326302e27282f";
            string result = cipher.Encrypt(source, key);

            Assert.AreEqual(expected, result);
        }
    }
}
