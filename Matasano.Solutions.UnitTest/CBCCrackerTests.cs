using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Matasano.Oracle;
using System.Text;

namespace Matasano.Cipher.AES.Tests
{
    [TestClass]
    public class CBCCrackerTests
    {
        [TestMethod]
        public void EncryptAndDecrypt()
        {
            string[] lines = File.ReadAllLines(@"TestFiles\AESCBC.txt");

            EncryptionOracleCBC oracle = new EncryptionOracleCBC();

            foreach (string line in lines)
            {
                string decoded = new Base64(line).Decode();
                var encrypted = oracle.Encrypt(decoded);
                string decrypted = oracle.Decrypt(encrypted.Item1, encrypted.Item2);

                Assert.AreEqual(decrypted, decoded);
            }
        }

        [TestMethod]
        public void BrokenPadding()
        {
            string[] lines = File.ReadAllLines(@"TestFiles\AESCBC.txt");
            int blockSize = 16;

            EncryptionOracleCBC oracle = new EncryptionOracleCBC();

            string line = lines[0];

            string decoded = new Base64(line).Decode();

            var encrypted = oracle.Encrypt(decoded);

            // Cracker
            var cracked = CrackCipherBlocks(blockSize, oracle, encrypted);

            Console.WriteLine("done");
        }

        private static string CrackCipherBlocks(int blockSize, EncryptionOracleCBC oracle, Tuple<Base64, string> encrypted)
        {
            var numberOfBlocks = encrypted.Item1.Decode().Length / blockSize;
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < numberOfBlocks - 1; i++)
            {
                Bytes rawEncrypted = new Bytes(encrypted.Item1.Decode().Substring(i * blockSize, 2 * blockSize));
                Bytes iv = new Bytes(encrypted.Item2);

                byte[] tampered = new byte[32];
                byte[] decryptedblock = new byte[16];
                byte[] validPadding = new byte[16];

                rawEncrypted.ToArray().CopyTo(tampered, 0);

                for (int byteNumber = 15; byteNumber >= 0; byteNumber--)
                {
                    int position = 16 - byteNumber;

                    for (int backFill = 15; backFill > byteNumber; backFill--)
                    {
                        tampered[backFill] = (byte)(validPadding[backFill] ^ position);
                    }

                    for (byte guess = 0; guess < 255; guess++)
                    {
                        tampered[byteNumber] = (byte)(guess ^ position);
                        if (oracle.IsValidPadding(new Hex(tampered), iv.ToString()))
                        {
                            validPadding[byteNumber] = (byte)(guess);
                            decryptedblock[byteNumber] = (byte)(rawEncrypted[byteNumber] ^ guess);
                            break;
                        }
                    }
                }
                sb.Append(new Bytes(decryptedblock).ToString());
            }

            return sb.ToString();
        }
    }
}
