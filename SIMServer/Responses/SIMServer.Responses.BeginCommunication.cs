namespace SIMServer.Responses
{
    public sealed class BeginCommunication
    {
        public BeginCommunication(string publicKey, int leaseDuration)
        {
            this.PublicKey = publicKey;
            this.LeaseDuration = leaseDuration;
        }

        public string PublicKey { get; private set; }

        public int LeaseDuration { get; private set; }
    }
}
