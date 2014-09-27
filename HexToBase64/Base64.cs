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
            return new Bytes(Convert.FromBase64String(_data)).ToHex();
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
