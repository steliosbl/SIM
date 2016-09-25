namespace SIMCommon.Responses
{
    public sealed class UserRef : Base
    {
        public UserRef(UserProfile profile) : base()
        {
            this.Profile = profile;
        }

        public UserProfile Profile { get; private set; }
    }
}
