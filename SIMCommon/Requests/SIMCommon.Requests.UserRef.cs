namespace SIMCommon.Requests
{
    using System.Collections.Generic;

    public sealed class UserRef : Base
    {
        public UserRef(string username) : base(true)
        {
            this.Username = username;
        }

        public string Username { get; private set; }
    }
}
