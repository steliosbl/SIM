namespace SIMCommon.Responses
{
    public class Authenticate : Base
    {
        public Authenticate(bool response) : base()
        {
            this.Response = response;
        }

        public bool Response { get; private set; }
    }
}
