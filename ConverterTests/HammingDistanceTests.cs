using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matasano;

namespace ConverterTests
{
    [TestClass]
    public class HammingDistanceTests
    {
        [TestMethod]
        public void FindDistancePerKeySize()
        {
            HammingDistance hd = new HammingDistance();
            string source = "abcdefghabcdefgh";
            int correctKeySize = 8;
            int hammingDistance = 0;

            var result = hd.FindDistancePerKeySize(2, 10, source);
            Assert.AreEqual(result[correctKeySize], hammingDistance);
        }

        [TestMethod]
        public void FindDistancePerKeySizeWith4BlockSize()
        {
            HammingDistance hd = new HammingDistance();
            string source = "abcdefghabcdefghabcdefghabcdefgh";
            int correctKeySize = 8;
            int hammingDistance = 0;

            var result = hd.FindDistancePerKeySize(2, 10, source, 4);
            Assert.AreEqual(result[correctKeySize], hammingDistance);
        }

        [TestMethod]
        public void FindDistancePerKeySizeWithInvalidBlockNumber()
        {
            HammingDistance hd = new HammingDistance();
            try
            {
                string source = "abcdef";
                var result = hd.FindDistancePerKeySize(2, 10, source, 3);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {

            }
        }

        [TestMethod]
        public void HammingDistanceOfInequalString()
        {
            HammingDistance hd = new HammingDistance();
            string str1 = "1";
            string str2 = "12";
            try
            {
                hd.Calculate(str1, str2);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {

            }
        }

        [TestMethod]
        public void HammingDistance()
        {
            HammingDistance hd = new HammingDistance();
            string str1 = "this is a test";
            string str2 = "wokka wokka!!!";
            int distance = 37;
            Assert.AreEqual(distance, hd.Calculate(str1, str2));
        }
    }
}
