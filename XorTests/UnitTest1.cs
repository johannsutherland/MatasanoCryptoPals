using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matasano;

namespace ConverterTests
{
    [TestClass]
    public class XorTests
    {
        [TestMethod]
        public void InequalLengths()
        {
            Converter c = new Converter();
            byte[] op1 = new byte[10];
            byte[] op2 = new byte[11];
            try
            {
                c.Xor(op1, op2);
            }
            catch (ArgumentException ex)
            {
                return;
            }

            Assert.Fail();
        }

        [TestMethod]
        public void OneByteBothZero()
        {
            Converter c = new Converter();
            byte[] op1 = new byte[1] { 0 };
            byte[] op2 = new byte[1] { 0 };
            byte[] expected = new byte[] { 0 };

            byte[] actual = c.Xor(op1, op2);

            Assert.AreEqual(expected[0], actual[0]);
        }

        [TestMethod]
        public void OneByteBothOne()
        {
            Converter c = new Converter();
            byte[] op1 = new byte[1] { 1 };
            byte[] op2 = new byte[1] { 1 };
            byte[] expected = new byte[] { 0 };

            byte[] actual = c.Xor(op1, op2);

            Assert.AreEqual(expected[0], actual[0]);
        }

        [TestMethod]
        public void Challenge2()
        {
            Converter c = new Converter();
            string op1 = "1c0111001f010100061a024b53535009181c";
            string op2 = "686974207468652062756c6c277320657965";
            string expected = "746865206b696420646f6e277420706c6179";

            string actual = c.Xor(op1, op2);

            Assert.AreEqual(expected[0], actual[0]);
        }
    }
}
