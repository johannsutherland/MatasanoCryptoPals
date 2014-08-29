using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matasano;
using System.Diagnostics;

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
            string[] result = cracker.BreakXorFile(@"Xor.txt", 2, 60, 4);
            Assert.AreEqual(expected, result[0]);
        }
    }
}
