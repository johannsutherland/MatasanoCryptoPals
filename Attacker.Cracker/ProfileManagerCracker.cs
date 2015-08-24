using System;

using Target;

namespace Attacker.Cracker
{
    public class ProfileManagerCracker
    {
        ProfileManager profileManager;

        public ProfileManagerCracker(ProfileManager profileManager)
        {
            this.profileManager = profileManager;
        }

        public void BreakProfileManager(string userEmail)
        {
            if (userEmail.Length != 13)
                throw new ArgumentOutOfRangeException("The email length must be 13.");

            // Get email=foooo@bar.com&uid=10&role=user
            profileManager.AddProfile(userEmail);
            var userRole = profileManager.Encrypted(userEmail);

            // Get email=fo@bar.comadmin            &uid=10&role=user
            string adminBlockEmail = "fo@bar.comadmin" + new string((char)11, 12);
            profileManager.AddProfile(adminBlockEmail);
            var adminBlock = profileManager.Encrypted(adminBlockEmail);

            var admin = adminBlock.Substring(16, 16);
            var updatedCookie = userRole.Substring(0, 32) + admin;

            profileManager.AddProfile(userEmail, updatedCookie);
        }
    }
}
