using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matasano
{
    public class HammingDistance
    {
        public int Calculate(string str1, string str2)
        {
            if (str1.Length != str2.Length)
                throw new ArgumentException("String lengths must be equal");

            var bytes1 = new BitArray(Encoding.Unicode.GetBytes(str1.ToCharArray()));
            var bytes2 = new BitArray(Encoding.Unicode.GetBytes(str2.ToCharArray()));

            var results = bytes1.Xor(bytes2);

            int difference = 0;

            foreach (bool bit in results)
            {
                difference += bit ? 1 : 0;
            }

            return difference;
        }

        public Dictionary<int, float> FindDistancePerKeySize(int startKeySize, int endKeySize, string source, int numberOfBlocks = 2)
        {
            if (numberOfBlocks % 2 != 0)
            {
                throw new ArgumentException("Number of blocks must be divisable by 2", "numberOfBlocks");
            }

            var result = new Dictionary<int, float>();

            for (int keySize = startKeySize; keySize <= endKeySize; keySize++)
            {
                if (numberOfBlocks * keySize > source.Length)
                {
                    break;
                }

                string[] str = new string[numberOfBlocks];
                for (int i = 0; i < numberOfBlocks; i++)
                {
                    str[i] = source.Substring(i * keySize, keySize);
                }

                int totalDistance = 0;
                for (int i = 0; i * 2 < str.Length; i++)
                {
                    totalDistance += this.Calculate(str[2 * i], str[2 * i + 1]);
                }

                result.Add(keySize, ((float)totalDistance / (float)keySize));
            }

            return result;
        }
    }
}
