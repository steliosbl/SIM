namespace SIMCommon.Responses
{
    public sealed class InitConnection : Base
    {
        public InitConnection(string publicKey, int leaseDuration) : base()
        {
            this.PublicKey = publicKey;
            this.LeaseDuration = leaseDuration;
        }

        public string PublicKey { get; private set; }

        public int LeaseDuration { get; private set; }
    }
}
