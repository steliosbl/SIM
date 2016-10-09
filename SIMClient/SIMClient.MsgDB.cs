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
            this.Connection.Open();
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

            using (var cmd = new SQLiteCommand("SELECT * FROM " + SIMCommon.Constants.SIMClientDatabaseMessageTable + " WHERE threadID = @id ORDER BY timestamp DEC LIMIT @limit OFFSET @offset;", this.Connection))
            {
                cmd.Prepare();
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

        public bool ThreadExists(int id)
        {
            using (var cmd = new SQLiteCommand("SELECT EXISTS(SELECT * FROM " + SIMCommon.Constants.SIMClientDatabaseMessageTable + " WHERE ID = @id LIMIT 1);", this.Connection))
            {
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@id", id);
                return System.Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        public Thread GetThread(int id)
        {
            Thread result = null;
            if (this.ThreadExists(id))
            {
                using (var cmd = new SQLiteCommand("SELECT * FROM " + SIMCommon.Constants.SIMClientDatabaseMessageTable + " WHERE ID = @id;", this.Connection))
                {
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@id", id);

                    result = SDatabase.SQLite.Convert.DeserializeObject<Thread>(cmd);
                }
            }

            return result;
        }

        public List<Thread> GetAllThreads()
        {
            var result = new List<Thread>();
            using (var cmd = new SQLiteCommand("SELECT * from " + SIMCommon.Constants.SIMClientDatabaseMessageTable + "", this.Connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        using (var dcmd = new SQLiteCommand("SELECT * FROM " + SIMCommon.Constants.SIMClientDatabaseMessageTable + " WHERE ID = @id", this.Connection))
                        {
                            cmd.Prepare();
                            cmd.Parameters.AddWithValue("@id", reader.GetInt32(0));

                            result.Add(SDatabase.SQLite.Convert.DeserializeObject<Thread>(dcmd));
                        }
                    }
                }
            }

            return result;
        }
    }
}
