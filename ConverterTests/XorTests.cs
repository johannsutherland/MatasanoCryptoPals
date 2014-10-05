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
            Bytes op1 = new Bytes(new byte[10]);
            Bytes op2 = new Bytes(new byte[11]);
            try
            {
                op1.Xor(op2);
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
            Bytes op1 = new Bytes(new byte[1] { 0 });
            Bytes op2 = new Bytes(new byte[1] { 0 });
            Bytes expected = new Bytes(new byte[] { 0 });

            Bytes actual = op1.Xor(op2);

            Assert.AreEqual(expected[0], actual[0]);
        }

        [TestMethod]
        public void OneByteBothOne()
        {
            Bytes op1 = new Bytes(new byte[1] { 1 });
            Bytes op2 = new Bytes(new byte[1] { 1 });
            Bytes expected = new Bytes(new byte[] { 0 });

            Bytes actual = op1.Xor(op2);

            Assert.AreEqual(expected[0], actual[0]);
        }

        [TestMethod]
        [TestCategory("Set 1 - Challenge 02")]
        public void Xor()
        {
            Hex op1 = new Hex("1c0111001f010100061a024b53535009181c");
            Hex op2 = new Hex("686974207468652062756c6c277320657965");
            Hex expected = new Hex("746865206b696420646f6e277420706c6179");

            Hex actual = op1.Xor(op2);

            Assert.AreEqual(expected, actual);
        }
    }
}
