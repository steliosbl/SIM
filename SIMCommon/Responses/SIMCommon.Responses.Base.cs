namespace SIMCommon.Responses
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
