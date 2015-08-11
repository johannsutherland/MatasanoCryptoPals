using Matasano;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Matasano.Tests
{
    [TestClass]
    public class HexTransposeTests
    {
        [TestMethod]
        public void Transpose3x4Blocks()
        {
            Hex source = new Hex("abcdabcdabcd");
            int keySize = 4;
            Hex[] transposed = { new Hex("ababab"), new Hex("cdcdcd") };

            Hex[] result = source.CreateAndTransposeBlocks(keySize);
            Assert.AreEqual(String.Join<Hex>(",", transposed), String.Join<Hex>(",", result));
        }

        [TestMethod]
        public void Transpose10x5Blocks()
        {
            Hex source = new Hex("0a0b0c0d0e0a0b0c0d0e0a0b0c0d0e0a0b0c0d0e0102030405");
            int keySize = 10;
            Hex[] transposed = { new Hex("0a0a0a0a01"), new Hex("0b0b0b0b02"), new Hex("0c0c0c0c03"), new Hex("0d0d0d0d04"), new Hex("0e0e0e0e05") };

            Hex[] result = source.CreateAndTransposeBlocks(keySize);
            Assert.AreEqual(String.Join<Hex>(",", transposed), String.Join<Hex>(",", result));
        }
    }
}
