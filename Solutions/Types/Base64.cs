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

        public Hex ToHex()
        {
            return this.ToBytes().ToHex();
        }

        public Bytes ToBytes()
        {
            return new Bytes(Convert.FromBase64String(_data));
        }

        public byte[] ToByteArray()
        {
            return this.ToBytes().ToArray();
        }

        public string Decode()
        {
            return this.ToBytes().ToString();
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
