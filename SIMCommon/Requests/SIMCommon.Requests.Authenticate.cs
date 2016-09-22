namespace SIMCommon.Requests
{
    public class Authenticate : Base
    {
        public Authenticate(string username, string password) : base(false)
        {
            this.Username = username;
            this.Password = password;
        }

        public string Username { get; private set; }

        public string Password { get; private set; }
    }
}