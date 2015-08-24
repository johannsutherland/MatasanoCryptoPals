using Microsoft.VisualStudio.TestTools.UnitTesting;

using Matasano;
using Target;
using Attacker.Cracker;

namespace Attacker.Cracker.Tests
{
    [TestClass]
    public class SubmissionManagerTests
    {
        [TestMethod]
        public void CantCreateAdmin()
        {
            SubmissionManager submissionManager = new SubmissionManager();
            string userInput = ";admin=true;";
            Base64 encrypted = submissionManager.Encrypt(userInput);
            bool isAdmin = submissionManager.IsAdmin(encrypted);

            Assert.IsFalse(isAdmin);
        }

        [TestMethod]
        [TestCategory("Set 2 - Challenge 16")]
        public void BreakSubmissionManager()
        {
            SubmissionManager submissionManager = new SubmissionManager();
            SubmissionManagerCracker cracker = new SubmissionManagerCracker(submissionManager);

            Base64 encrypted = cracker.CraftBreakingBlock();

            bool isAdmin = submissionManager.IsAdmin(encrypted);

            Assert.IsTrue(isAdmin);
        }
    }
}
