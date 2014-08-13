using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matasano;

namespace ConverterTests
{
    [TestClass]
    public class ConvertFromHexToBase64Tests
    {
        [TestMethod]
        public void ConvertOneCharacter()
        {
            string hex = "68";
            string base64 = "aA==";
            Converter c = new Converter();
            Assert.AreEqual(c.HexToBase64(hex), base64);
        }

        [TestMethod]
        public void ConvertTwoCharactersWithoutHyphen()
        {
            string hex = "6869";
            string base64 = "aGk=";
            Converter c = new Converter();
            Assert.AreEqual(c.HexToBase64(hex), base64);
        }

        [TestMethod]
        public void Challenge1()
        {
            string hex = "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d";
            string base64 = "SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t";
            Converter c = new Converter();
            Assert.AreEqual(c.HexToBase64(hex), base64);
        }
    }
}
