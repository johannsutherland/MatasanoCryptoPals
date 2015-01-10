﻿using System;
namespace Matasano
{
    public interface IEncryptionOracle
    {
        Base64 EncryptConsistentKey(string data, Base64 unknownString, int startIndex = 0);
    }
}
