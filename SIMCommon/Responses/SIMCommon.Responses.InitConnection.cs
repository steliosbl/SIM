namespace SIMCommon.Responses
{
    public sealed class InitConnection : Base
    {
        public InitConnection(bool success)
        {
            this.Success = success;
            this.PublicKey = null;
            this.LeaseDuration = -1;
        }

        [Newtonsoft.Json.JsonConstructor]
        public InitConnection(bool success, string publicKey, int leaseDuration) : base()
        {
            this.Success = success;
            this.PublicKey = publicKey;
            this.LeaseDuration = leaseDuration;
        }

        public bool Success { get; private set; }

        public string PublicKey { get; private set; }

        public int LeaseDuration { get; private set; }
    }
}
