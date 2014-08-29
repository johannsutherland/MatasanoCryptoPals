using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matasano;
using System.Diagnostics;

namespace ConverterTests
{
    [TestClass]
    public class BreakXorCipherTests
    {
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
        public void BreakXorFile()
        {
            XorCipher cipher = new XorCipher();
            string expected = "Terminator X: Bring the noise";
            string[] result = cipher.BreakXorFile(@"Xor.txt", 2, 60, 4);
            Assert.AreEqual(expected, result[0]);
            Debug.WriteLine(result[1]);
        }
    }
}
