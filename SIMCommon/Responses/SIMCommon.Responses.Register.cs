namespace SIMCommon.Responses
{
    public sealed class Register : Base
    {
        public Register(bool success) : base()
        {
            this.Success = success;
        }

        public bool Success { get; private set; }
    }
}
