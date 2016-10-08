namespace SIMCommon.Requests
{
    using System;

    public class Base
    {
        public Base(bool loggedInRequirement)
        {
            this.RequestType = this.GetType();
            this.LoggedInRequirement = loggedInRequirement;
        }

        [Newtonsoft.Json.JsonConstructor]
        public Base(Type requestType, bool loggedInRequirement)
        {
            this.RequestType = requestType;
            this.LoggedInRequirement = loggedInRequirement;
        }

        public Type RequestType { get; private set; }

        public bool LoggedInRequirement { get; private set; }
    }
}
