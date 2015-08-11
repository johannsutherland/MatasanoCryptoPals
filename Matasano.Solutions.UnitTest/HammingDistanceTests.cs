using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Attacker.Cipher.Tests
{
    [TestClass]
    public class HammingDistanceTests
    {
        [TestMethod]
        public void FindDistancePerKeySize()
        {
            string source = "abcdefghabcdefgh";
            int correctKeySize = 8;
            int hammingDistance = 0;

            var result = HammingDistance.FindDistancePerKeySize(2, 10, source);
            Assert.AreEqual(result[correctKeySize], hammingDistance);
        }

        [TestMethod]
        public void FindDistancePerKeySizeWith4BlockSize()
        {
            string source = "abcdefghabcdefghabcdefghabcdefgh";
            int correctKeySize = 8;
            int hammingDistance = 0;

            var result = HammingDistance.FindDistancePerKeySize(2, 10, source, 4);
            Assert.AreEqual(result[correctKeySize], hammingDistance);
        }

        [TestMethod]
        public void FindDistancePerKeySizeWithInvalidBlockNumber()
        {
            try
            {
                string source = "abcdef";
                var result = HammingDistance.FindDistancePerKeySize(2, 10, source, 3);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                return;
            }
        }

        [TestMethod]
        public void HammingDistanceOfInequalString()
        {
            string str1 = "1";
            string str2 = "12";
            try
            {
                HammingDistance.Calculate(str1, str2);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                return;
            }
        }

        [TestMethod]
        public void CalculateHammingDistance()
        {
            string str1 = "this is a test";
            string str2 = "wokka wokka!!!";
            int distance = 37;
            Assert.AreEqual(distance, HammingDistance.Calculate(str1, str2));
        }
    }
}
