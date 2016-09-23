namespace SIMCommon.Responses
{
    public sealed class Send : Base
    {
        public Send(bool success) : base()
        {
            this.Success = success;
        }

        public bool Success { get; private set; }
    }
}
