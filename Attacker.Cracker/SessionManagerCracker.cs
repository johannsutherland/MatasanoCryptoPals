﻿using System;
using System.Text;

using Matasano;
using Matasano.Cipher.AES;

using Target;
using System.Collections.Generic;
using System.Linq;

namespace Attacker.Cracker
{
    public class SessionManagerCracker
    {
        SessionManager sessionManager;
        int blockSize = 16;

        List<char> ValidCharacters = new List<char>();

        public SessionManagerCracker(SessionManager sessionManager)
        {
            this.sessionManager = sessionManager;

            ValidCharacters.AddRange(Enumerable.Range('A', 'Z' - 'A' + 1).Select(x => (char)x).ToList());
            ValidCharacters.AddRange(Enumerable.Range('a', 'z' - 'a' + 1).Select(x => (char)x).ToList());
            ValidCharacters.AddRange(Enumerable.Range('0', '9' - '0' + 1).Select(x => (char)x).ToList());
            ValidCharacters.Add(' ');
            ValidCharacters.Add(',');
            ValidCharacters.Add('\'');
        }

        public string Break(EncryptedData encryptedData)
        {
            var crackedFirstBlock = CrackFirstCipherBlock(encryptedData);
            var crackedRest = CrackCipherBlocks(encryptedData);

            var cracked = crackedFirstBlock + crackedRest;

            return cracked;
        }

        private string CrackFirstCipherBlock(EncryptedData encryptedData)
        {
            StringBuilder sb = new StringBuilder();

            Bytes rawEncrypted = new Bytes(encryptedData.Data.ToString().Substring(0, blockSize));
            Bytes iv = new Bytes(encryptedData.IV);

            byte[] tampered = new byte[blockSize];
            byte[] decryptedblock = new byte[blockSize];
            byte[] validPadding = new byte[blockSize];

            iv.ToArray().CopyTo(tampered, 0);

            for (int byteNumber = blockSize - 1; byteNumber >= 0; byteNumber--)
            {
                int position = blockSize - byteNumber;

                for (int backFill = blockSize - 1; backFill > byteNumber; backFill--)
                {
                    tampered[backFill] = (byte)(validPadding[backFill] ^ position);
                }

                for (byte guess = 0; guess < 255; guess++)
                {
                    tampered[byteNumber] = (byte)(guess ^ position);
                    try
                    {
                        sessionManager.ReceiveEncryptedCookie(new Hex(rawEncrypted), new Bytes(tampered).ToString());
                        validPadding[byteNumber] = (byte)(guess);
                        decryptedblock[byteNumber] = (byte)(iv[byteNumber] ^ guess);

                        if (!ValidCharacters.Contains((char)decryptedblock[byteNumber]))
                        {
                            continue;
                        }

                        break;
                    }
                    catch (InvalidPaddingException)
                    {
                        // Try next one
                    }
                }
            }
            sb.Append(new Bytes(decryptedblock).ToString());

            return sb.ToString();
        }

        private string CrackCipherBlocks(EncryptedData encrypted)
        {
            var numberOfBlocks = encrypted.Data.ToString().Length / blockSize;
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < numberOfBlocks - 1; i++)
            {
                Bytes rawEncrypted = new Bytes(encrypted.Data.ToString().Substring(i * blockSize, 2 * blockSize));
                Bytes iv = new Bytes(encrypted.IV);

                byte[] tampered = new byte[blockSize * 2];
                byte[] decryptedblock = new byte[blockSize];
                byte[] validPadding = new byte[blockSize];

                rawEncrypted.ToArray().CopyTo(tampered, 0);

                for (int byteNumber = blockSize - 1; byteNumber >= 0; byteNumber--)
                {
                    int position = blockSize - byteNumber;

                    for (int backFill = blockSize - 1; backFill > byteNumber; backFill--)
                    {
                        tampered[backFill] = (byte)(validPadding[backFill] ^ position);
                    }

                    for (byte guess = 0; guess < 255; guess++)
                    {
                        tampered[byteNumber] = (byte)(guess ^ position);
                        try
                        {
                            sessionManager.ReceiveEncryptedCookie(new Hex(tampered), iv.ToString());
                            validPadding[byteNumber] = (byte)(guess);
                            decryptedblock[byteNumber] = (byte)(rawEncrypted[byteNumber] ^ guess);

                            if (!ValidCharacters.Contains((char)decryptedblock[byteNumber]))
                            {
                                continue;
                            }

                            break;
                        }
                        catch
                        {
                            // Try next one
                        }
                    }
                }
                sb.Append(new Bytes(decryptedblock).ToString());
            }

            return sb.ToString();
        }
    }
}
