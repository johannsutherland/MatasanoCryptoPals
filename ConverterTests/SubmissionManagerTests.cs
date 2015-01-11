using Matasano;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace ConverterTests
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
