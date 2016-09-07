﻿namespace SIMServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MySql.Data.MySqlClient;
    using SDatabase.MySQL;

    public class SIMAuthDB
    {
        public SIMAuthDB(ConnectionData connectionData)
        {
            this.ConnectionString = new ConnectionString(connectionData);
            this.Connection = new MySqlConnection(this.ConnectionString.Text);
            this.Connection.Open();
        }

        private ConnectionString ConnectionString { get; set; }
        
        private MySqlConnection Connection { get; set; }

        public int GetLastUserID()
        {
            using (var cmd = new MySqlCommand("SELECT MAX (ID) FROM @table;", this.Connection))
            {
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@table", SIMCommon.GlobalConstants.SIMAuthDBUserTable);
                try
                {
                    return System.Convert.ToInt32(cmd.ExecuteScalar());
                }
                // Caused if there are no users in the table.
                catch (FormatException)
                {
                    return 0;
                }
            }
        }

        public int GetUserID(string username)
        {
            if (this.UserExists(username))
            {
                using (var cmd = new MySqlCommand("SELECT * FROM @table WHERE Username = @username;", this.Connection))
                {
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@table", SIMCommon.GlobalConstants.SIMAuthDBUserTable);
                    cmd.Parameters.AddWithValue("@username", username);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        return System.Convert.ToInt32(rdr["ID"]);
                    }
                }
            }
            else
            {
                throw new ArgumentException("User does not exist.");
            }
        }

        public string GetUsername(int id)
        {
            if (this.UserExists(id))
            {
                using (var cmd = new MySqlCommand("SELECT * FROM @table WHERE ID = @id;", this.Connection))
                {
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@table", SIMCommon.GlobalConstants.SIMAuthDBUserTable);
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        return rdr["Username"].ToString();
                    }
                }
            }
            else
            {
                throw new ArgumentException("User does not exist.");
            }
        }

        public bool UserExists(int id)
        {
            using (var cmd = new MySqlCommand("SELECT EXISTS(SELECT * FROM @table WHERE ID = @id LIMIT 1);", this.Connection))
            {
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@table", SIMCommon.GlobalConstants.SIMAuthDBUserTable);
                cmd.Parameters.AddWithValue("@id", id);
                return System.Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        public bool UserExists(string username)
        {
            using (var cmd = new MySqlCommand("SELECT EXISTS(SELECT * FROM @table WHERE Username = @username LIMIT 1);", this.Connection))
            {
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@table", SIMCommon.GlobalConstants.SIMAuthDBUserTable);
                cmd.Parameters.AddWithValue("@username", username);
                return System.Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        public SIMUser GetUser(int id)
        {
            if (this.UserExists(id))
            {
                using (var cmd = new MySqlCommand("SELECT * FROM @table WHERE ID = @id;", this.Connection))
                {
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@table", SIMCommon.GlobalConstants.SIMAuthDBUserTable);
                    cmd.Parameters.AddWithValue("@id", id);
                    return SDatabase.MySQL.Convert.DeserializeObject<SIMUser>(cmd);
                }
            }
            else
            {
                throw new ArgumentException("User does not exist.");
            }
        }
    }
}
