namespace SIMCommon.Responses
{
    public class Authenticate : Base
    {
        public Authenticate(bool success) : base()
        {
            this.Success = success;
        }

        public bool Success { get; private set; }
    }
}
