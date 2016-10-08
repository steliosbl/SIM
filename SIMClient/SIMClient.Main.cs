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
            this.CurrentThread = null;
            this.Server = new SIMClient.Server(address);
            this.GetClock = new System.Threading.Thread(() => this.GetMessages());
            if (this.Server.InitConnection())
            {
                this.GetClock.Start();
            }
            else
            {
                throw new InitializationFailiureException();
            }
        }

        public Server Server { get; private set; }

        public MsgDB Database { get; private set; }

        public SIMCommon.UserProfile CurrentUser { get; private set; }

        public Thread CurrentThread { get; private set; }

        public System.Threading.Thread GetClock { get; private set; }

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
                this.Server.GetProfiles();
                return true;
            }

            return false;
        }

        public void SignOut()
        {
            this.Server.SignOut();
            this.CurrentUser = null;
        }

        public bool Send(string message, int recipient)
        {
            if (this.LoggedIn)
            {
                var msg = new SIMCommon.Message(this.CurrentUser.ID, recipient, message);
                if (this.Server.Send(msg))
                {
                    this.Database.WriteMessage(msg);
                    return true;
                }

                return false;
            }

            return false;
        }

        public bool LoadThread(int id)
        {
            if (this.Database.ThreadExists(id))
            {
                this.CurrentThread = this.Database.GetThread(id);
            }

            return false;
        }

        public void ReloadThread()
        {
            this.LoadThread(this.CurrentThread.ID);
        }

        private void GetMessages()
        {
            while (true)
            {
                var threadIDs = new List<int>();
                foreach (SIMCommon.Message message in this.Server.Get())
                {
                    this.Database.WriteMessage(message);
                    if (!threadIDs.Contains(message.ThreadID))
                    {
                        threadIDs.Add(message.ThreadID);
                    }
                }

                if (this.CurrentThread != null)
                {
                    if (threadIDs.Contains(this.CurrentThread.ID))
                    {
                        this.ReloadThread();
                    }
                }

                System.Threading.Thread.Sleep(SIMCommon.Constants.SIMClientGetClockDelay);
            }
        }

        public class InitializationFailiureException : Exception
        {
            public InitializationFailiureException()
            {
            }
        }
    }
}
