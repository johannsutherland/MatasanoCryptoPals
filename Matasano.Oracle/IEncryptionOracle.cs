using System;

namespace Matasano.Oracle
{
    public interface IEncryptionOracle
    {
        Base64 EncryptConsistentKey(string data, Base64 unknownString, int startIndex = 0);
        Tuple<Base64, string> EncryptWithRandomPadding(string data);
    }
}
