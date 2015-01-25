using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

using Matasano.Cracker;

namespace Matasano.Cracker.Tests
{
    [TestClass]
    public class XorCrackerTests
    {
        [TestMethod]
        [TestCategory("Set 1 - Challenge 06")]
        public void BreakXorFile()
        {
            XorCracker cracker = new XorCracker();
            string expected = "Terminator X: Bring the noise";
            Base64 data = new Base64(String.Join("", File.ReadAllLines(@"TestFiles\Xor.txt")));
            string[] result = cracker.BreakXorFile(data, 2, 60, 4);
            Assert.AreEqual(expected, result[0]);
        }
    }
}
