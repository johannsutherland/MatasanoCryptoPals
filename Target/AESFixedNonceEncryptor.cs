using Matasano;
using Matasano.Cipher.AES;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Target
{
    class AESFixedNonceEncryptor
    {
        AESCipherCTR cipher;

        public AESFixedNonceEncryptor()
        {
            cipher = new AESCipherCTR(0);
        }

        public Bytes[] EncryptFile(string filename)
        {
            var lines = Base64.FromFile(new FileInfo(filename));
            var numberOfLines = lines.Length;

            Bytes[] encryptedLines = new Bytes[numberOfLines];

            for (int line = 0; line < numberOfLines; line++)
            {
                encryptedLines[line] = new Bytes(cipher.Encrypt(lines[line].Decode()));
                cipher.ResetNonce();
            }

            return encryptedLines;
        }

    }
}
