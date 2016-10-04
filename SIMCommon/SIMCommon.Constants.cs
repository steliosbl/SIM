namespace SIMCommon
{
    public static class Constants
    {
        // Common
        public const int TcpStreamByteSize = 512;
        public const int LeaseMonitorDelay = 100;

        // SIMClient
        public const int SIMClientPort = 71913;
        public const int SIMClientMsgLoadLimit = 50;
        public const int SIMClientThreadOffset = 10;
        public const int SIMClientGetClockDelay = 100;
        public const int SIMClientGetClockStartDelay = 1000;
        public const string SIMClientDatabaseFilename = "MsgDB.db";
        public const string SIMClientDatabaseMessageTable = "SIMMESSAGES";
        public const string SIMClientDatabaseThreadTable = "SIMTHREADS";

        // SIMServer
        public const int SIMServerPort = 13197;
        public const string SIMServerAuthDatabaseUserTable = "SIMUSERS";
        public const string SIMServerConfigFilename = "SIMServer.cfg";
        public const string SIMServerInvalidRequestResponse = "NULL";
        public const string SIMServerBacklogFilename = "SIMServer.backlog.json";

        public const string SIMServerLoggerDelimiter = "\n ---------------------------------- \n";
        public const string SIMServerLoggerSeverity0 = "[INFO] ";
        public const string SIMServerLoggerSeverity1 = "[WARN] ";
        public const string SIMServerLoggerSeverity2 = "[SEVERE] ";
        public const string SIMServerLoggerSeverity3 = "[CRITICAL] ";
    }
}