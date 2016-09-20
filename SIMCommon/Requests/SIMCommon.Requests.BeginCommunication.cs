namespace SIMCommon.Requests
{
    public sealed class BeginCommunication : Base
    {
        public BeginCommunication(string publicKey) : base()
        {
            this.PublicKey = publicKey;
        }

        public string PublicKey { get; private set; }
    }
}