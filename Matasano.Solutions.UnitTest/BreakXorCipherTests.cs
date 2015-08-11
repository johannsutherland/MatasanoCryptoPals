using Microsoft.VisualStudio.TestTools.UnitTesting;

using Matasano;

namespace Attacker.Cipher.Xor.Tests
{
    [TestClass]
    public class BreakXorCipherTests
    {
        [TestMethod]
        public void Decrypt()
        {
            XorCracker cracker = new XorCracker();
            Hex source = new Hex("1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736");
            string expected = "Cooking MC's like a pound of bacon";
            char expectedChar = 'X';
            var result = cracker.TryDecrypt(source);
            Assert.AreEqual(expected, result[expectedChar]);
        }

        [TestMethod]
        [TestCategory("Set 1 - Challenge 03")]
        public void DecryptAndFindKey()
        {
            XorCracker cracker = new XorCracker(); 
            Hex source = new Hex("1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736");
            char expected = 'X';
            var key = cracker.TryDecryptAndFindKey(source);
            Assert.AreEqual(expected, key);
        }

        [TestMethod]
        [TestCategory("Set 1 - Challenge 04")]
        public void DecryptFile()
        {
            XorCracker cracker = new XorCracker(); 
            string expected = "Now that the party is jumping" + (char)10;
            string result = cracker.TryDecryptFile(@"TestFiles\XorEncrypted.txt");
            Assert.AreEqual(expected, result);
        }
    }
}
