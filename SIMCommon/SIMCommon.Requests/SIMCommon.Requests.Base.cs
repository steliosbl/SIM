namespace SIMCommon.Requests
{
    using System;

    public class Base
    {
        public Base()
        {
            this.RequestType = this.GetType();
        }

        public Type RequestType { get; private set; }
    }
}
