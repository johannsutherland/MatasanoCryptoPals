using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matasano;
using System.Diagnostics;
using System.IO;

namespace ConverterTests
{
    [TestClass]
    public class CrackerTests
    {
        [TestMethod]
        public void BreakXorFile()
        {
            Cracker cracker = new Cracker(new XorCipher());
            string expected = "Terminator X: Bring the noise";
            Base64 data = new Base64(String.Join("", File.ReadAllLines(@"Xor.txt")));
            string[] result = cracker.BreakXorFile(data, 2, 60, 4);
            Assert.AreEqual(expected, result[0]);
        }
    }
}
