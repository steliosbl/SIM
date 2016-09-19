namespace SIMCommon
{
    public static class Constants
    {
        public const int TcpStreamByteSize = 512;
        public const int SIMServerPort = 13197;
        public const int SIMClientPort = 71913;
        public const int SIMClientMsgLoadLimit = 50;
        public const string SIMClientDatabaseFilename = "MsgDB.db";
        public const string SIMClientDatabaseMessageTable = "SIMMESSAGES";
        public const string SIMClientDatabaseThreadTable = "SIMTHREADS";
        public const string SIMServerAuthDBUserTable = "SIMUSERS";
        public const string SIMServerConfigFilename = "SIMServer.cfg";
        public const string SIMServerInvalidRequestResponse = "NULL";
        public const string SIMServerBacklogFilename = "SIMServer.backlog.json";
    }
}