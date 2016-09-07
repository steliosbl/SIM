namespace SIMCommon.Requests
{
    public sealed class SendMessage : Base
    {
        public SendMessage(Message message) : base()
        {
            this.Message = message;
        }

        public Message Message { get; private set; }
    }
}