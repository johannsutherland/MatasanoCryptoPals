using System.Collections.Generic;

using Matasano.Cipher.AES;

namespace Target
{
    public class ProfileManager
    {
        private Dictionary<string, ValuePairParser> profiles = new Dictionary<string, ValuePairParser>();
        AESCipherECB cipher = new AESCipherECB();
        AESCipherHelper helper = new AESCipherHelper();

        string defaultProfile = "&uid=10&role=user";
        string key;

        public ProfileManager()
        {
            key = helper.GenerateKey().ToString();
        }

        public void AddProfile(string email)
        {
            string sanitisedEmail = email.Replace("&", "").Replace("=", "");
            ValuePairParser cookie = new ValuePairParser("email=" + sanitisedEmail + defaultProfile);

            profiles.Add(sanitisedEmail, cookie);
        }

        public void AddProfile(string email, string encoded)
        {
            string decoded = cipher.Decrypt(key, encoded);

            if (profiles.ContainsKey(email))
            {
                profiles[email] = new ValuePairParser(decoded);
            }
            else
            {
                profiles.Add(email, new ValuePairParser(decoded));
            }
        }

        public ValuePairParser this[string email]
        {
            get { return profiles[email]; }
        }

        public string Encrypted(string email)
        {
            return cipher.Encrypt(profiles[email].ToString()).Decode();
        }
    }
}
