namespace SIMCommon.Requests
{
    public sealed class Encrypted : Base
    {
        public Encrypted(byte[] encryptedSessionKey, string encryptedRequest) : base()
        {
            this.EncryptedSessionKey = encryptedSessionKey;
            this.EncryptedRequest = encryptedRequest;
        }

        public byte[] EncryptedSessionKey { get; private set; }

        public string EncryptedRequest { get; private set; }
    }
}
