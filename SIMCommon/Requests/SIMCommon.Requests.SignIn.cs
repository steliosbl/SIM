namespace SIMCommon.Requests
{
    public sealed class SignIn : Authenticate
    {
        public SignIn(string username, string password) : base(username, password)
        {
        }
    }
}
