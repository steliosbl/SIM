namespace SIMServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Backlog
    {
        public Backlog()
        {
            this.Messages = new Dictionary<int, List<SIMCommon.Message>>();
        }

        public Backlog(Dictionary<int, List<SIMCommon.Message>> messages)
        {
            this.Messages = messages;
        }

        public Dictionary<int, List<SIMCommon.Message>> Messages { get; private set; }
    }
}
