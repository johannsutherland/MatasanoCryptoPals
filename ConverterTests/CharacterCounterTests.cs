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
            CharacterCounter cc = new CharacterCounter();
            var result = cc.Frequency("AB");

            Assert.AreEqual(result['A'], 1);
            Assert.AreEqual(result['B'], 1);
        }

        [TestMethod]
        public void A2B4()
        {
            CharacterCounter cc = new CharacterCounter();
            var result = cc.Frequency("ABABBB");

            Assert.AreEqual(result['A'], 2);
            Assert.AreEqual(result['B'], 4);
        }

        [TestMethod]
        public void Transpose3x4Blocks()
        {
            string source = "abcdabcdabcd";
            int keySize = 4;
            string[] transposed = { "ababab", "cdcdcd" };

            CharacterCounter cc = new CharacterCounter();

            string[] result = cc.CreateAndTransposeBlocks(source, keySize);
            Assert.AreEqual(String.Join(",", transposed), String.Join(",", result));
        }

        [TestMethod]
        public void Transpose10x5Blocks()
        {
            string source = "0a0b0c0d0e0a0b0c0d0e0a0b0c0d0e0a0b0c0d0e0102030405";
            int keySize = 10;
            string[] transposed = { "0a0a0a0a01", "0b0b0b0b02", "0c0c0c0c03", "0d0d0d0d04", "0e0e0e0e05" };

            CharacterCounter cc = new CharacterCounter();

            string[] result = cc.CreateAndTransposeBlocks(source, keySize);
            Assert.AreEqual(String.Join(",", transposed), String.Join(",", result));
        }
    }
}
