using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matasano;
using System.Collections.Generic;

namespace ConverterTests
{
    [TestClass]
    public class CookieTest
    {
        [TestMethod]
        public void CookieParserFromString()
        {
            var valuePairs = "foo=bar&baz=qux&zap=zazzle";

            ValuePairParser parser = new ValuePairParser(valuePairs);

            Assert.AreEqual(parser["foo"], "bar");
            Assert.AreEqual(parser["baz"], "qux");
            Assert.AreEqual(parser["zap"], "zazzle");
        }

        [TestMethod]
        public void CookieParserFromInvalidString()
        {
            var valuePairs = "foo=bar=1&baz=qux&zap=zazzle";

            ValuePairParser parser = new ValuePairParser(valuePairs);

            try
            {
                var value = parser["foo"];
                Assert.Fail("Key should not exist");
            }
            catch (KeyNotFoundException ex)
            {

            }
            catch (Exception ex)
            {
                Assert.Fail("Invalid exception");
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
