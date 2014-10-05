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
            var cookie = "foo=bar&baz=qux&zap=zazzle";

            Cookie cookieParser = new Cookie(cookie);

            Assert.AreEqual(cookieParser["foo"], "bar");
            Assert.AreEqual(cookieParser["baz"], "qux");
            Assert.AreEqual(cookieParser["zap"], "zazzle");
        }

        [TestMethod]
        public void CookieParserFromInvalidString()
        {
            var cookie = "foo=bar=1&baz=qux&zap=zazzle";

            Cookie cookieParser = new Cookie(cookie);

            try
            {
                var value = cookieParser["foo"];
                Assert.Fail("Key should not exist");
            }
            catch (KeyNotFoundException ex)
            {

            }
            catch (Exception ex)
            {
                Assert.Fail("Invalid exception");
            }
            
            Assert.AreEqual(cookieParser["baz"], "qux");
            Assert.AreEqual(cookieParser["zap"], "zazzle");
        }

        [TestMethod]
        public void CookieToString()
        {
            var str = "foo=bar&baz=qux&zap=zazzle";

            Cookie cookie = new Cookie(str);

            Assert.AreEqual(str, cookie.ToString());
        }

        [TestMethod]
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
