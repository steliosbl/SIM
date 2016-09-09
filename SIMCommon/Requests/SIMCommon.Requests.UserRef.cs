namespace SIMCommon.Requests
{
    using System.Collections.Generic;

    public sealed class UserRef : Base
    {
        public UserRef(int id) : base()
        {
            this.ID = id;
        }

        public int ID { get; private set; }
    }
}
