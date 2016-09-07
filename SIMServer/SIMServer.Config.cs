namespace SIMServer
{
    using System;
    using Newtonsoft.Json;
    using SDatabase;

    public class Config
    {   
        [JsonConstructor]
        public Config(SDatabase.MySQL.ConnectionData connectionInfo)
        {
            this.ConnectionInfo = connectionInfo;
        }

        public SDatabase.MySQL.ConnectionData ConnectionInfo { get; private set; }
    }
}
