namespace SIMCommon.Responses
{
    public sealed class Encrypted : Base
    {
        public Encrypted(byte[] encryptedSessionKey, string encryptedResponse) : base()
        {
            this.EncryptedSessionKey = encryptedSessionKey;
            this.EncryptedResponse = encryptedResponse;
        }

        public byte[] EncryptedSessionKey { get; private set; }

        public string EncryptedResponse { get; private set; }
    }
}