using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matasano
{
    public class Bytes
    {
        private byte[] _data;

        public Bytes(byte[] data)
        {
            _data = data;
        }

        public Bytes(string data)
        {
            _data = new byte[data.Length];

            for (int n = 0; n < data.Length; n++)
            {
                _data[n] = (byte)data[n];
            }
        }

        public Hex ToHex()
        {
            return new Hex(_data);
        }

        public Base64 ToBase64()
        {
            return new Base64(Convert.ToBase64String(_data, Base64FormattingOptions.None));
        }

        public byte[] ToArray()
        {
            return _data;
        }

        public int Length
        {
            get { return _data.Length; }
        }

        public void Add(byte[] array)
        {
            byte[] newArray = new byte[_data.Length + array.Length];
            _data.CopyTo(newArray, 0);
            array.CopyTo(newArray, _data.Length);

            _data = newArray;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in _data)
            {
                sb.Append(c);
            }
            return sb.ToString();
        }

        public byte this[int index]
        {
            get { return _data[index]; }
        }

        public Bytes Xor(Bytes bytes)
        {
            if (_data.Length != bytes.ToArray().Length)
            {
                throw new ArgumentException("Lengths must be equal");
            }

            byte[] result = new byte[_data.Length];
            for (int i = 0; i < _data.Length; i++)
            {
                result[i] = (byte)(_data[i] ^ bytes.ToArray()[i]);
            }

            return new Bytes(result);
        }
    }
}
