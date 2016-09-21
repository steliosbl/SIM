namespace SIMCommon.Responses
{
    using System;

    public class Base
    {
        public Base()
        {
            this.ResponseType = this.GetType();
        }

        public Type ResponseType { get; private set; }
    }
}
