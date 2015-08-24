using Microsoft.VisualStudio.TestTools.UnitTesting;

using Attacker.Target;
using Target;

namespace Matasano.Target.Tests
{
    [TestClass]
    public class CBCCrackerTests
    {
        [TestMethod]
        public void EncryptAndDecrypt()
        {
            SessionManager sessionManager = new SessionManager();

            var encryptedCookie = sessionManager.GetEncryptedCookie();
            sessionManager.ReceiveEncryptedCookie(encryptedCookie.Item1, encryptedCookie.Item2);

            Assert.AreEqual(sessionManager.decoded, sessionManager.decrypted);
        }

        [TestMethod]
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
