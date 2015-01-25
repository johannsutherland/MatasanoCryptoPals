using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matasano.Cracker
{
    public class RandomPrefixAESCracker : AESCracker
    {
        int? _randomPrefixLength;

        public int RandomPrefixLength
        {
            get
            {
                if (!_randomPrefixLength.HasValue)
                {
                    FindRandomPrefixLength();
                }
                return _randomPrefixLength.Value;
            }
        }

        public RandomPrefixAESCracker(Base64 unknownString)
            : base(unknownString)
        {
            base.eo = new EncryptionOracleWithRandomPrefix();
        }

        public int FindRandomPrefixLength()
        {
            int blockSize = 16;
            _randomPrefixLength = 16;

            EncryptionOracleWithRandomPrefix encryption = new EncryptionOracleWithRandomPrefix();

            string[] firstBlock = new string[blockSize];
            firstBlock[0] = encryption.EncryptConsistentKey("", base.unknownString).Substring(0, blockSize).ToString();

            for (int i = 1; i < blockSize; i++)
            {
                firstBlock[i] = encryption.EncryptConsistentKey(new String('A', i), base.unknownString).Substring(0, blockSize).ToString();

                if (firstBlock[i] == firstBlock[i - 1])
                {
                    _randomPrefixLength = 17 - i;
                    break;
                }
            }

            return _randomPrefixLength.Value;
        }

        public string Break()
        {
            return base.Break(this.RandomPrefixLength);
        }
    }
}
