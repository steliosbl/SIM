namespace SIMCommon.Responses
{
    public sealed class Ping : Base
    {
        public Ping(bool accepting) : base()
        {
            this.AcceptingConnections = accepting;
        }

        public bool AcceptingConnections { get; private set; }
    }
}
