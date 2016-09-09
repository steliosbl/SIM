namespace SIMCommon.Requests
{
    public sealed class Send : Base
    {
        public Send(Message message) : base()
        {
            this.Message = message;
        }

        public Message Message { get; private set; }
    }
}