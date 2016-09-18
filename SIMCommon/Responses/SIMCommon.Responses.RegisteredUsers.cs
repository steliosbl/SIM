namespace SIMCommon.Responses
{
    using System;
    using System.Collections.Generic;

    public sealed class RegisteredUsers : Base
    {
        public RegisteredUsers(List<UserProfile> users) : base()
        {
            this.Users = users;
        }

        public List<UserProfile> Users { get; private set; }
    }
}
