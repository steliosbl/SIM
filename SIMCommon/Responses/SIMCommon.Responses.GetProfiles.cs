namespace SIMCommon.Responses
{
    using System;
    using System.Collections.Generic;

    public sealed class GetProfiles : Base
    {
        public GetProfiles(List<UserProfile> users) : base()
        {
            this.Users = users;
        }

        public List<UserProfile> Users { get; private set; }
    }
}
