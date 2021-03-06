﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

using Attacker.Cracker;
using Target;

namespace Attacker.Cracker.Tests
{
    [TestClass]
    public class CBCCrackerTests
    {
        [TestMethod]
        public void EncryptAndDecrypt()
        {
            SessionManager sessionManager = new SessionManager();

            var encryptedCookie = sessionManager.GetEncryptedCookie();
            sessionManager.ReceiveEncryptedCookie(encryptedCookie.Data, encryptedCookie.IV);

            Assert.AreEqual(sessionManager.decoded, sessionManager.decrypted);
        }

        [TestMethod]
        [TestCategory("Set 3 - Challenge 17")]
        public void CrackUsingInvalidPadding()
        {
            SessionManager sessionManager = new SessionManager();

            var encryptedCookie = sessionManager.GetEncryptedCookie();
            SessionManagerCracker cracker = new SessionManagerCracker(sessionManager);
            var cracked = cracker.Break(encryptedCookie);

            Assert.AreEqual(sessionManager.decoded, cracked.Substring(0, sessionManager.decoded.Length));
        }
    }
}
