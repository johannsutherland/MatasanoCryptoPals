namespace Matasano
{
    public struct EncryptedData
    {
        public Hex Data { get; set; }
        public string IV { get; set; }

        public EncryptedData(Hex data, string iv)
        {
            Data = data;
            IV = iv;
        }
    }
}
