using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matasano;

namespace ConverterTests
{
    [TestClass]
    public class AESTests
    {
        [TestMethod]
        public void DecryptAES()
        {
            AESCipher aes = new AESCipher();
            aes.Decrypt("YELLOW SUBMARINE", "AES.txt");
        }
    }
}
