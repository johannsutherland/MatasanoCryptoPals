using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matasano;
using System.IO;

namespace ConverterTests
{
    [TestClass]
    public class XorCrackerTests
    {
        [TestMethod]
        public void BreakXorFile()
        {
            XorCracker cracker = new XorCracker();
            string expected = "Terminator X: Bring the noise";
            Base64 data = new Base64(String.Join("", File.ReadAllLines(@"Xor.txt")));
            string[] result = cracker.BreakXorFile(data, 2, 60, 4);
            Assert.AreEqual(expected, result[0]);
        }
    }
}
