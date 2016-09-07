namespace SIMServer
{
    using System;
    using Newtonsoft.Json;
    using SDatabase;

    public class SIMServerConfig
    {
        public SDatabase.MySQL.ConnectionData ConnectionInfo { get; private set; }
        
        [JsonConstructor]
        public SIMServerConfig(SDatabase.MySQL.ConnectionData connectionInfo)
        {
            this.ConnectionInfo = connectionInfo;
        }
    }
}
