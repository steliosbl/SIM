namespace SIMClient
{
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SDatabase.SQLite;

    public class MsgDB
    {
        public MsgDB()
        {
            this.ConnectionString = new ConnectionString(SIMCommon.Constants.SIMClientDatabaseFilename, 3);
            this.Connection = new SQLiteConnection(this.ConnectionString.Text);
        }

        private ConnectionString ConnectionString { get; set; }

        private SQLiteConnection Connection { get; set; }

        public void WriteMessage(SIMCommon.Message message)
        {
            SDatabase.SQLite.Convert.SerializeObject(this.Connection, message, SIMCommon.Constants.SIMClientDatabaseMessageTable);
        }

        public List<SIMCommon.Message> ReadThread(int threadID, int offset = 0)
        {
            var result = new List<SIMCommon.Message>();

            using (var cmd = new SQLiteCommand("SELECT * FROM @table WHERE threadID = @id ORDER BY timestamp DEC LIMIT @limit OFFSET @offset;", this.Connection))
            {
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@table", SIMCommon.Constants.SIMClientDatabaseMessageTable);
                cmd.Parameters.AddWithValue("@id", threadID);
                cmd.Parameters.AddWithValue("@limit", SIMCommon.Constants.SIMClientMsgLoadLimit);
                cmd.Parameters.AddWithValue("@offset", offset);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new SIMCommon.Message(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetDateTime(3)));
                    }
                }
            }

            return result;
        }

        public Thread GetThread(int id)
        {
            Thread result = null;
            using (var cmd = new SQLiteCommand("SELECT * FROM @table WHERE ID = @id;", this.Connection))
            {
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@table", SIMCommon.Constants.SIMClientDatabaseThreadTable);
                cmd.Parameters.AddWithValue("@id", id);

                result = SDatabase.SQLite.Convert.DeserializeObject<Thread>(cmd);
            }

            return result;
        }
    }
}
