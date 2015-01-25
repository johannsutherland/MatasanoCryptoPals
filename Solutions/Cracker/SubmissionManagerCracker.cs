using Matasano.ExternalSystem;

namespace Matasano.Cracker
{
    public class SubmissionManagerCracker
    {
        SubmissionManager submissionManager;

        public SubmissionManagerCracker(SubmissionManager submissionManager)
        {
            this.submissionManager = submissionManager;
        }

        public Base64 CraftBreakingBlock()
        {
            string craftedBlock = ":admin<true:1234";
            byte[] encryptedBytes = submissionManager.Encrypt(craftedBlock).ToByteArray();

            encryptedBytes[16] = (byte)(encryptedBytes[16] ^ 1);
            encryptedBytes[22] = (byte)(encryptedBytes[22] ^ 1);
            encryptedBytes[27] = (byte)(encryptedBytes[27] ^ 1);

            return new Bytes(encryptedBytes).ToBase64();
        }
    }
}
