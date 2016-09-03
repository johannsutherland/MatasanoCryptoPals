using System;
using System.Text;

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

        public static implicit operator Hex(Bytes bytes)
        {
            return new Hex(bytes._data);
        }

        public static implicit operator Base64(Bytes bytes)
        {
            return new Base64(Convert.ToBase64String(bytes._data, Base64FormattingOptions.None));
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
            set { _data[index] = value; }
        }

        public Bytes Xor(Bytes bytes)
        {
            int minLength = Math.Min(_data.Length, bytes.Length);

            byte[] result = new byte[minLength];
            for (int i = 0; i < minLength; i++)
            {
                result[i] = (byte)(_data[i] ^ bytes.ToArray()[i]);
            }

            return new Bytes(result);
        }

        public Bytes Substring(int start, int length)
        {
            byte[] b = new byte[length];
            for (int i = 0; i < length; i++)
            {
                b[i] = _data[i + start];
            }
            return new Bytes(b);
        }

        public Bytes SafeSubstring(int start, int length)
        {
            if (start > _data.Length)
            {
                return new Bytes(String.Empty);
            }
            else if (_data.Length < start + length)
            {
                return Substring(start, _data.Length - start);
            }
            else
            {
                return Substring(start, length);
            }
        }
    }
}
