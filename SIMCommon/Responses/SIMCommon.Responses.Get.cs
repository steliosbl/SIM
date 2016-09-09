namespace SIMCommon.Responses
{
    using System;
    using System.Collections.Generic;

    public sealed class Get : Base
    {
        public Get(List<SIMCommon.Message> messages) : base()
        {
            this.Messages = messages;
        }

        public List<SIMCommon.Message> Messages { get; private set; }
    }
}
