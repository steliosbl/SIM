namespace SIMServer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Logger
    {
        private readonly string filename;
        private readonly bool outputToConsole;

        public Logger(string filename, bool outputToConsole)
        {
            this.filename = filename;
            this.outputToConsole = outputToConsole;
            File.AppendAllText(this.filename, SIMCommon.Constants.SIMServerLoggerDelimiter);
        }

        public void Log(string msg, int severity = 0)
        {
            string[] tags = { SIMCommon.Constants.SIMServerLoggerSeverity0, SIMCommon.Constants.SIMServerLoggerSeverity1, SIMCommon.Constants.SIMServerLoggerSeverity2, SIMCommon.Constants.SIMServerLoggerSeverity3 };

            string message = "[" + DateTime.Now.ToString() + "] " + tags[severity] + msg;

            File.AppendAllText(this.filename, message);
            if (this.outputToConsole)
            {
                Console.WriteLine(message);
            }
        }
    }
}
