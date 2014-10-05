using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matasano;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace ConverterTests
{
    [TestClass]
    public class CrackerTests
    {
        Base64 unknownString = new Base64("Um9sbGluJyBpbiBteSA1LjAKV2l0aCBteSByYWctdG9wIGRvd24gc28gbXkgaGFpciBjYW4gYmxvdwpUaGUgZ2lybGllcyBvbiBzdGFuZGJ5IHdhdmluZyBqdXN0IHRvIHNheSBoaQpEaWQgeW91IHN0b3A/IE5vLCBJIGp1c3QgZHJvdmUgYnkK");
        int blockSize = 16;

        [TestMethod]
        public void FindBlockSize()
        {
            Cracker cracker = new Cracker(unknownString);
            Assert.AreEqual(blockSize, cracker.FindBlockSize());
        }

        [TestMethod]
        public void DetectEncryption()
        {
            Cracker cracker = new Cracker(unknownString);
            Assert.IsTrue(cracker.IsECB(blockSize));
        }

        [TestMethod]
        public void BreakMessage()
        {
            string expected = "Rollin' in my 5.0\nWith my rag-top down so my hair can blow\nThe girlies on standby waving just to say hi\nDid you stop? No, I just drove by\n";
            Cracker cracker = new Cracker(unknownString);
            string actual = cracker.Break();

            Assert.AreEqual(expected, actual);
        }
    }
}
