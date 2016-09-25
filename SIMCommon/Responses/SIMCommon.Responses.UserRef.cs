namespace SIMCommon.Responses
{
    public sealed class UserRef : Base
    {
        public UserRef(string nickname) : base()
        {
            this.Nickname = nickname;
        }

        public string Nickname { get; private set; }
    }
}
