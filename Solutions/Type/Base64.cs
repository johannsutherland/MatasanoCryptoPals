using System;
using System.IO;

namespace Matasano
{
    public class Base64
    {
        private string _data;

        public Base64(string data)
        {
            _data = data;
        }

        public Base64(FileInfo file)
        {
            _data = String.Join("", File.ReadAllLines(file.FullName));
        }

        public static implicit operator Hex(Base64 base64)
        {
            return ((Bytes)base64);
        }

        public static implicit operator Bytes(Base64 base64)
        {
            return new Bytes(Convert.FromBase64String(base64._data));
        }

        public byte[] ToByteArray()
        {
            return ((Bytes)this).ToArray();
        }

        public string Decode()
        {
            return ((Bytes)this).ToString();
        }

        public Base64 Substring(int startIndex)
        {
            return new Base64(_data.Substring(startIndex));
        }

        public Base64 Substring(int startIndex, int length)
        {
            return new Base64(_data.Substring(startIndex, length));
        }

        public override bool Equals(object obj)
        {
            return _data.Equals(obj.ToString());
        }

        public override string ToString()
        {
            return _data;
        }

        public override int GetHashCode()
        {
            return _data.GetHashCode();
        }
    }
}
