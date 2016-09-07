namespace SIMServer
{
    using System;
    using SCrypto;

    public class Client
    {
        public Client(string address)
        {
            this.Address = address;
            this.PGPClient = new SCrypto.PGP.SPGP();
            this.User = null;
            this.LeaseStart = DateTime.Now;
        }

        public DateTime LeaseStart { get; private set; }

        public SCrypto.PGP.SPGP PGPClient { get; private set; }

        public string Address { get; private set; }

        public User User { get; private set; }

        public void LoadUser(User user)
        {
            this.User = user;
        }

        public void RenewLease()
        {
            this.LeaseStart = DateTime.Now;
        }
    }
}
