using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Matasano.Helper.Tests
{
    [TestClass]
    public class CharacterCounterTests
    {
        [TestMethod]
        public void A1B1()
        {
            var result = CharacterCounter.Frequency("AB");

            Assert.AreEqual(result['A'], 1);
            Assert.AreEqual(result['B'], 1);
        }

        [TestMethod]
        public void A2B4()
        {
            var result = CharacterCounter.Frequency("ABABBB");

            Assert.AreEqual(result['A'], 2);
            Assert.AreEqual(result['B'], 4);
        }
    }
}
