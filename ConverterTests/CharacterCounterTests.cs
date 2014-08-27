using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matasano;

namespace ConverterTests
{
    [TestClass]
    public class CharacterCounterTests
    {
        [TestMethod]
        public void A1B1()
        {
            CharacterManager cc = new CharacterManager();
            var result = cc.Frequency("AB");

            Assert.AreEqual(result['A'], 1);
            Assert.AreEqual(result['B'], 1);
        }

        [TestMethod]
        public void A2B4()
        {
            CharacterManager cc = new CharacterManager();
            var result = cc.Frequency("ABABBB");

            Assert.AreEqual(result['A'], 2);
            Assert.AreEqual(result['B'], 4);
        }

        [TestMethod]
        public void Decrypt()
        {
            XorCipher cipher = new XorCipher();
            string source = "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736";
            string expected = "Cooking MC's like a pound of bacon";
            char expectedChar = 'X';
            var result = cipher.Decrypt(source);
            Assert.AreEqual(expected, result[expectedChar]);
        }

        [TestMethod]
        public void DecryptAndFindKey()
        {
            XorCipher cipher = new XorCipher();
            string source = "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736";
            char expected = 'X';
            var key = cipher.DecryptAndFindKey(source);
            Assert.AreEqual(expected, key);
        }

        [TestMethod]
        public void DecryptFile()
        {
            string expected = "Now that the party is jumping" + (char)10;
            XorCipher cipher = new XorCipher();
            string result = cipher.DecryptFile("encrypted.txt");
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

        [TestMethod]
        public void HammingDistanceOfInequalString()
        {
            Converter conv = new Converter();
            string str1 = "1";
            string str2 = "12";
            try
            {
                conv.HammingDistance(str1, str2);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                
            }
        }
      
        [TestMethod]
        public void HammingDistance()
        {
            Converter conv = new Converter();
            string str1 = "this is a test";
            string str2 = "wokka wokka!!!";
            int distance = 37;
            Assert.AreEqual(distance, conv.HammingDistance(str1, str2));
        }
    }
}
