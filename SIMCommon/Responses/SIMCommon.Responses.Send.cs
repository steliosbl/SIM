namespace SIMCommon.Responses
{
    public sealed class Send : Base
    {
        public Send(bool result) : base()
        {
            this.Result = result;
        }

        public bool Result { get; private set; }
    }
}
