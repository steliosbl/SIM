namespace SIMServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MySql.Data.MySqlClient;
    using SDatabase.MySQL;

    public class AuthDB
    {
        public AuthDB(ConnectionData connectionData)
        {
            this.ConnectionString = new ConnectionString(connectionData);
            this.Connection = new MySqlConnection(this.ConnectionString.Text);
            this.Connection.Open();
        }

        private ConnectionString ConnectionString { get; set; }
        
        private MySqlConnection Connection { get; set; }

        public int GetLastUserID()
        {
            using (var cmd = new MySqlCommand("SELECT MAX(ID) FROM " + SIMCommon.Constants.SIMServerAuthDatabaseUserTable + ";", this.Connection))
            {
                try
                {
                    return System.Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (System.InvalidCastException)
                {
                    // Caused if there are no users in the table.
                    return 0;
                }
            }
        }

        public int GetUserID(string username)
        {
            if (this.UserExists(username))
            {
                using (var cmd = new MySqlCommand("SELECT * FROM " + SIMCommon.Constants.SIMServerAuthDatabaseUserTable + " WHERE Username = @username;", this.Connection))
                {
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@username", username);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            return System.Convert.ToInt32(rdr["ID"]);
                        }

                        throw new ArgumentException("User does not exist.");
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
                using (var cmd = new MySqlCommand("SELECT * FROM " + SIMCommon.Constants.SIMServerAuthDatabaseUserTable + " WHERE ID = @id;", this.Connection))
                {
                    cmd.Prepare();
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
            using (var cmd = new MySqlCommand("SELECT EXISTS(SELECT * FROM " + SIMCommon.Constants.SIMServerAuthDatabaseUserTable + " WHERE ID = @id LIMIT 1);", this.Connection))
            {
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@id", id);
                return System.Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        public bool UserExists(string username)
        {
            using (var cmd = new MySqlCommand("SELECT EXISTS(SELECT * FROM " + SIMCommon.Constants.SIMServerAuthDatabaseUserTable + " WHERE Username = @username LIMIT 1);", this.Connection))
            {
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@username", username);
                return System.Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        public User GetUser(int id)
        {
            if (this.UserExists(id))
            {
                using (var cmd = new MySqlCommand("SELECT * FROM " + SIMCommon.Constants.SIMServerAuthDatabaseUserTable + " WHERE ID = @id;", this.Connection))
                {
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@id", id);
                    return SDatabase.MySQL.Convert.DeserializeObject<User>(cmd);
                }
            }
            else
            {
                throw new ArgumentException("User does not exist.");
            }
        }

        public List<SIMCommon.UserProfile> GetProfiles()
        {
            var result = new List<SIMCommon.UserProfile>();
            using (var cmd = new MySqlCommand("SELECT ID, Nickname FROM " + SIMCommon.Constants.SIMServerAuthDatabaseUserTable + ";", this.Connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new SIMCommon.UserProfile(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }

            return result;
        }

        public void AddUser(User user)
        {
            SDatabase.MySQL.Convert.SerializeObject(this.Connection, user, SIMCommon.Constants.SIMServerAuthDatabaseUserTable);
        }
    }
}
