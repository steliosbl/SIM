﻿namespace SIMServer
{
    class SIMUser
    {
        public SIMUser(int id, string username, string rawPassword)
        {
            this.ID = id;
            this.Usernaame = username;
            this.GenerateSalt();
            this.Password = this.HashPassword(rawPassword);
            this.Nickname = username;
        }

        public SIMUser(int id, string username, string password, string salt, string nickname)
        {
            this.ID = id;
            this.Usernaame = username;
            this.Password = password;
            this.Salt = salt;
            this.Nickname = nickname;
        }

        public int ID { get; private set; }

        public string Usernaame { get; private set; }

        public string Password { get; private set; }

        public string Salt { get; private set; }

        public string Nickname { get; private set; }

        private void GenerateSalt()
        {
            this.Salt = System.Guid.NewGuid().ToString();
        }

        private bool Authenticate(string inputPassword)
        {
            return this.HashPassword(inputPassword) == this.Password;
        }

        private string HashPassword(string inputPassword)
        {
            if (!string.IsNullOrWhiteSpace(inputPassword) && !string.IsNullOrEmpty(inputPassword))
            {
                return SCrypto.Hash.SHA_256.GetDigest(this.Salt + inputPassword + this.Salt);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
