using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matasano;

namespace ConverterTests
{
    [TestClass]
    public class BreakXorCipherTests
    {
        [TestMethod]
        public void FindDistancePerKeySize()
        {
            XorCipher xor = new XorCipher();
            string source = "abcdefghabcdefgh";
            int correctKeySize = 8;
            int hammingDistance = 0;

            var result = xor.FindDistancePerKeySize(2, 10, source);
            Assert.AreEqual(result[correctKeySize], hammingDistance);
        }

        [TestMethod]
        public void FindDistancePerKeySizeWith4BlockSize()
        {
            XorCipher xor = new XorCipher();
            string source = "abcdefghabcdefghabcdefghabcdefgh";
            int correctKeySize = 8;
            int hammingDistance = 0;

            var result = xor.FindDistancePerKeySize(2, 10, source, 4);
            Assert.AreEqual(result[correctKeySize], hammingDistance);
        }

        [TestMethod]
        public void FindDistancePerKeySizeWithInvalidBlockNumber()
        {
            XorCipher xor = new XorCipher();
            try
            {
                string source = "abcdef";
                var result = xor.FindDistancePerKeySize(2, 10, source, 3);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {

            }
        }

        [TestMethod]
        public void Transpose3x4Blocks()
        {
            string source = "abcdabcdabcd";
            int keySize = 4;
            string[] transposed = { "ababab", "cdcdcd" };

            XorCipher xor = new XorCipher();

            string[] result = xor.CreateAndTransposeBlocks(source, keySize);
            Assert.AreEqual(String.Join(",", transposed), String.Join(",", result));
        }

        [TestMethod]
        public void Transpose10x5Blocks()
        {
            string source = "0a0b0c0d0e0a0b0c0d0e0a0b0c0d0e0a0b0c0d0e0102030405";
            int keySize = 10;
            string[] transposed = { "0a0a0a0a01", "0b0b0b0b02", "0c0c0c0c03", "0d0d0d0d04", "0e0e0e0e05" };

            XorCipher xor = new XorCipher();

            string[] result = xor.CreateAndTransposeBlocks(source, keySize);
            Assert.AreEqual(String.Join(",", transposed), String.Join(",", result));
        }

        [TestMethod]
        public void BreakFile()
        {
            XorCipher cipher = new XorCipher();
            cipher.BreakXorFile(@"6.txt");
        }
    }
}
