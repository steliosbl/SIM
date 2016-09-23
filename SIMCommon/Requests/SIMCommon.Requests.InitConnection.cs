namespace SIMCommon.Requests
{
    public sealed class InitConnection : Base
    {
        public InitConnection(string publicKey) : base(false)
        {
            this.PublicKey = publicKey;
        }

        public string PublicKey { get; private set; }
    }
}