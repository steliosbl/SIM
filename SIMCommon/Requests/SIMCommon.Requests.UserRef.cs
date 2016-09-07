namespace SIMCommon.Requests
{
    using System.Collections.Generic;

    public sealed class UserRef : Base
    {
        public UserRef(List<string> usernames) : base()
        {
            this.Usernames = usernames;
        }

        public List<string> Usernames { get; private set; }
    }
}
