using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matasano.Cipher.AES
{
    public class AESCipherCTR : AESCipherECB
    {
        private string key;
        private long nonce;
        private byte[] blockCounter = new byte[16];
        
        public AESCipherCTR(string key, long nonce) : base()
        {
            this.key = key;
            this.nonce = nonce;
        }

        public AESCipherCTR(long nonce) : base()
        {
            this.nonce = nonce;
            key = helper.GenerateKey().ToString();
        }

        internal string GetNextCounter()
        {
            BitConverter.GetBytes(nonce).CopyTo(blockCounter, 8);
            nonce++;
            return new Bytes(blockCounter).ToString();
        }

        public string Decrypt(string data)
        {
            StringBuilder decrypted = new StringBuilder();

            foreach (string block in data.SplitByLength(this.blockSize))
            {
                var keystream = new Bytes(base.Encrypt(this.key, this.GetNextCounter()).Decode());
                decrypted.Append(keystream.Xor(new Bytes(block)));
            }

            return decrypted.ToString();
        }

        public new string Encrypt(string data)
        {
            return this.Decrypt(data);
        }

        public void ResetNonce()
        {
            nonce = 0;
        }
    }
}
