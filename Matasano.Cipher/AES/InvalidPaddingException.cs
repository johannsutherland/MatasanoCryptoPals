using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matasano.Cipher.AES
{
    public class InvalidPaddingException : Exception
    {
        public InvalidPaddingException(string message) : base(message)
        {

        }
    }
}
