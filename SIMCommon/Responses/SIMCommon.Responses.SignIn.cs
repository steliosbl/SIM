namespace SIMCommon.Responses
{
    public sealed class SignIn : Base
    {
        public SignIn(bool result) : base()
        {
            this.Result = result;
        }

        public bool Result { get; private set; }
    }
}
