﻿using System;

using Matasano;
using Matasano.Cipher.AES;

namespace Target
{
    public class SubmissionManager
    {
        AESCipherCBC cipher = new AESCipherCBC();
        AESCipherHelper helper = new AESCipherHelper();

        string prepend = "comment1=cooking%20MCs;userdata=";
        string append = ";comment2=%20like%20a%20pound%20of%20bacon";
        string key;
        string iv;

        public SubmissionManager()
        {
            key = helper.GenerateKey().ToString();
            iv = new String('\0', 16);
        }

        public Base64 Encrypt(string userInput)
        {
            string sanitisedUserInput = userInput.Replace(";", String.Empty).Replace("=", String.Empty);
            string padded = prepend + sanitisedUserInput + append;

            return cipher.Encrypt(key, padded, iv);
        }

        public bool IsAdmin(Base64 encrypted)
        {
            string values = cipher.Decrypt(key, encrypted, iv);
            string adminKey = "admin";

            ValuePairParser dictionaryManager = new ValuePairParser(values, ';');
            return dictionaryManager.Contains(adminKey) && dictionaryManager[adminKey] == "true";
        }
    }
}
