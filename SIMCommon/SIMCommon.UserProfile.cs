namespace SIMCommon
{
    using System;
    using Newtonsoft.Json;

    public class UserProfile
    {
        public UserProfile(int id, string nickname)
        {
            this.ID = id;
            this.Nickname = nickname;
            this.Active = false;
        }

        [JsonConstructor]
        public UserProfile(int id, string nickname, bool active)
        {
            this.ID = id;
            this.Nickname = nickname;
            this.Active = active;
        }

        public int ID { get; private set; }

        public string Nickname { get; private set; }

        public bool Active { get; private set; }
    }
}
