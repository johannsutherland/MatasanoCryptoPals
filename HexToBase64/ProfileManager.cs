using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matasano
{
    public class Cookie
    {
        private Dictionary<string, string> values = new Dictionary<string, string>();

        public Cookie(string cookie)
        {
            cookie.Split('&').ToList().ForEach(token =>
                {
                    var kvp = token.Split('=');
                    if (kvp.Length == 2) values.Add(kvp[0], kvp[1]);
                });
        }

        public string this[string key]
        {
            get { return values[key]; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            values.ToList().ForEach(kvp => sb.Append(kvp.Key + "=" + kvp.Value + "&"));
            string result = sb.ToString();
            return result.Substring(0, result.Length - 1);
        }
    }

    public class ProfileManager
    {
        private Dictionary<string, Cookie> profiles = new Dictionary<string, Cookie>();
        AESCipher cipher = new AESCipher();
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
            Cookie cookie = new Cookie("email=" + sanitisedEmail + defaultProfile);

            profiles.Add(sanitisedEmail, cookie);
        }

        public void AddProfile(string email, string encoded)
        {
            string decoded = cipher.DecryptECB(key, encoded);

            if (profiles.ContainsKey(email))
            {
                profiles[email] = new Cookie(decoded);
            }
            else
            {
                profiles.Add(email, new Cookie(decoded));
            }
        }

        public Cookie this[string email]
        {
            get { return profiles[email]; }
        }

        public string Encrypted(string email)
        {
            return cipher.EncryptECB(profiles[email].ToString()).Decode();
        }
    }
}
