namespace SIMServer
{
    using System;
    using Newtonsoft.Json;
    using SDatabase;

    public class Config
    {   
        [JsonConstructor]
        public Config(SDatabase.MySQL.ConnectionData connectionInfo, int leaseDuration)
        {
            this.ConnectionInfo = connectionInfo;
            this.LeaseDuration = leaseDuration;
        }

        public SDatabase.MySQL.ConnectionData ConnectionInfo { get; private set; }

        public int LeaseDuration { get; private set; }
    }
}
