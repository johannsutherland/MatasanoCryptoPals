using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using Target;
using Attacker.Cracker;

namespace Attacker.Cracker.Tests
{
    [TestClass]
    public class ProfileManagerTests
    {
        [TestMethod]
        [TestCategory("Set 2 - Challenge 13")]
        public void BreakProfileManager()
        {
            ProfileManager profileManger = new ProfileManager();
            string userRoleEmail = "foooo@bar.com";

            ProfileManagerCracker pmc = new ProfileManagerCracker(profileManger);
            pmc.BreakProfileManager(userRoleEmail);

            Assert.AreEqual("admin", profileManger[userRoleEmail]["role"]);
        }
    }
}
