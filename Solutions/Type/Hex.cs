using System;
using System.Linq;
using System.Text;

namespace Matasano
{
    public class Hex
    {
        private string _data;

        public enum InputFormat
        {
            Hex,
            String
        }

        public Hex(string data)
        {
            _data = data;
        }

        public Hex(string data, InputFormat inputFormat)
        {
            switch (inputFormat)
            {
                case InputFormat.String:
                    this.FromString(data);
                    break;
                case InputFormat.Hex:
                    _data = data;
                    break;
            }
        }

        public Hex(byte[] data)
        {
            this.CreateFromBytes(data);
        }

        public Hex(Bytes data)
        {
            this.CreateFromBytes(data.ToArray());
        }

        private void CreateFromBytes(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            data.ToList().ForEach(x => sb.Append(x.ToString("x2")));
            _data = sb.ToString();
        }

        private void FromString(string data)
        {
            StringBuilder result = new StringBuilder();
            foreach (char c in data)
            {
                result.Append(((byte)c).ToString("x2"));
            }
            _data = result.ToString();
        }

        public int Length
        {
            get
            {
                return _data.Length / 2;
            }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            int n = 0;

            while (n * 2 + 2 <= _data.Length)
            {
                result.Append((char)int.Parse(_data.Substring(n * 2, 2), System.Globalization.NumberStyles.HexNumber));
                n++;
            }

            return result.ToString();
        }

        public Bytes ToBytes()
        {
            byte[] bytes = new byte[_data.Length / 2];
            int n = 0;

            while (n * 2 + 2 <= _data.Length)
            {
                bytes[n] = (byte)int.Parse(_data.Substring(n * 2, 2), System.Globalization.NumberStyles.HexNumber);
                n++;
            }

            return new Bytes(bytes);
        }

        public Base64 ToBase64()
        {
            Bytes bytes = this.ToBytes();
            return new Base64(Convert.ToBase64String(bytes.ToArray(), Base64FormattingOptions.None));
        }

        public Hex Xor(Hex hex)
        {
            Bytes b1 = this.ToBytes();
            Bytes b2 = hex.ToBytes();
            return new Hex(b1.Xor(b2));
        }

        public override bool Equals(object obj)
        {
            return this.ToString().Equals(obj.ToString());
        }

        public override int GetHashCode()
        {
            return _data.GetHashCode();
        }

        public Hex[] CreateAndTransposeBlocks(int keySize)
        {
            string[] blocks = _data.SplitByLength(keySize).ToArray();
            string[] transposed = new string[keySize / 2];
            Hex[] result = new Hex[transposed.Length];

            for (int i = 0; i < transposed.Length; i++)
            {
                foreach (string block in blocks)
                {
                    if (2 * i + 1 < block.Length)
                    {
                        transposed[i] += block[2 * i].ToString() + block[2 * i + 1].ToString();
                    }
                }
            }

            for (int i = 0; i < transposed.Length; i++)
            {
                result[i] = new Hex(transposed[i]);
            }

            return result;
        }
    }
}
