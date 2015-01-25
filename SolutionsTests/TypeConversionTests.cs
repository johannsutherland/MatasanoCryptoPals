using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matasano;

namespace Matasano.Tests
{
    [TestClass]
    public class TypeConversionTests
    {
        [TestMethod]
        public void ConvertOneCharacter()
        {
            Hex hex = new Hex("68");
            Base64 base64 = new Base64("aA==");
            Assert.AreEqual(hex.ToBase64(), base64);
        }

        [TestMethod]
        public void ConvertTwoCharactersWithoutHyphen()
        {
            Hex hex = new Hex("6869");
            Base64 base64 = new Base64("aGk=");
            Assert.AreEqual(hex.ToBase64(), base64);
        }

        [TestMethod]
        public void Base64ToHex()
        {
            Hex hex = new Hex("68");
            Base64 base64 = new Base64("aA==");
            Assert.AreEqual(base64.ToHex(), hex);
        }

        [TestMethod]
        [TestCategory("Set 1 - Challenge 01")]
        public void HexToBase64()
        {
            Hex hex = new Hex("49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d");
            Base64 base64 = new Base64("SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t");
            Assert.AreEqual(hex.ToBase64(), base64);
        }
    }
}
