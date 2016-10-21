using System;

namespace Matasano.Cipher.AES
{
    public class InvalidPaddingException : Exception
    {
        public InvalidPaddingException() : base("Invalid Padding")
        {

        }
    }
}
