namespace SIMClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    public class Main
    {
        public Main(IPAddress address)
        {
            this.Database = new SIMClient.MsgDB();
            this.CurrentUser = null;
            this.Server = new SIMClient.Server(address);
        }

        public Server Server { get; private set; }

        public MsgDB Database { get; private set; }

        public SIMCommon.UserProfile CurrentUser { get; private set; }

        public bool LoggedIn
        {
            get
            {
                return this.CurrentUser == null;
            }
        }

        public bool SignIn(string username, string password)
        {
            if (this.Server.SignIn(username, password))
            {
                this.CurrentUser = this.Server.UserRef(username);
                return true;
            }

            return false;
        }

        public bool Send(string message, int recipient)
        {
            if (this.LoggedIn)
            {
                var msg = new SIMCommon.Message(this.CurrentUser.ID, recipient, message);
                return this.Server.Send(msg);
            }

            return false;
        }
    }
}
