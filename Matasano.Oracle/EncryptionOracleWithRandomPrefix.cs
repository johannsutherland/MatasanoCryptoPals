namespace Matasano.Oracle
{
    public class EncryptionOracleWithRandomPrefix : EncryptionOracle, IEncryptionOracle
    {
        Bytes randomPrefix;

        public int RandomPrefixLength
        {
            get
            {
                return randomPrefix.ToString().Length;
            }
        }

        public EncryptionOracleWithRandomPrefix() : base()
        {
            randomPrefix = base.helper.GenerateRandomLengthKey();
        }

        public new Base64 EncryptConsistentKey(string data, Base64 unknownString, int startIndex = 0)
        {
            return base.EncryptConsistentKey(randomPrefix.ToString() + data, unknownString, startIndex);
        }
    }
}
