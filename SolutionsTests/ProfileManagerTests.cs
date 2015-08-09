using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using Matasano.Helper;
using Matasano.Cracker;

namespace Matasano.ExternalSystem.Tests
{
    [TestClass]
    public class ProfileManagerTests
    {
        [TestMethod]
        public void ValuePairParserFromString()
        {
            var valuePairs = "foo=bar&baz=qux&zap=zazzle";

            ValuePairParser parser = new ValuePairParser(valuePairs);

            Assert.AreEqual(parser["foo"], "bar");
            Assert.AreEqual(parser["baz"], "qux");
            Assert.AreEqual(parser["zap"], "zazzle");
        }

        [TestMethod]
        public void ValuePairParserFromInvalidString()
        {
            var valuePairs = "foo=bar=1&baz=qux&zap=zazzle";

            ValuePairParser parser = new ValuePairParser(valuePairs);

            try
            {
                var value = parser["foo"];
                Assert.Fail("Key should not exist");
            }
            catch (KeyNotFoundException)
            {

            }
            catch (Exception)
            {
                Assert.Fail("Invalid exception");
                throw;
            }
            
            Assert.AreEqual(parser["baz"], "qux");
            Assert.AreEqual(parser["zap"], "zazzle");
        }

        [TestMethod]
        public void ValuePairToString()
        {
            var valuePairs = "foo=bar&baz=qux&zap=zazzle";

            ValuePairParser parser = new ValuePairParser(valuePairs);

            Assert.AreEqual(valuePairs, parser.ToString());
        }

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
