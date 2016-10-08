namespace SIMClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Newtonsoft.Json;
    using SCrypto;

    public class Server
    {
        public Server(IPAddress address)
        {
            this.Address = address;
            this.ServerPublicKey = null;
            this.LeaseDuration = 0;
            this.PGPClient = new SCrypto.PGP.SPGP();

            this.LeaseMonitor = new System.Threading.Thread(() => this.RunLeaseMonitor());
        }

        public IPAddress Address { get; private set; }

        public string ServerPublicKey { get; private set; }

        public int LeaseDuration { get; private set; }

        public DateTime LeaseStart { get; private set; }

        public SCrypto.PGP.SPGP PGPClient { get; private set; }

        public System.Threading.Thread LeaseMonitor { get; private set; }

        public List<SIMCommon.UserProfile> Profiles { get; private set; }

        public void EndConnection()
        {
            var request = new SIMCommon.Requests.EndConnection();
            this.SendEncryptedRequest(request);
            this.LeaseMonitor.Abort();
        }

        public List<SIMCommon.Message> Get()
        {
            try
            {
                var response = JsonConvert.DeserializeObject<SIMCommon.Responses.Get>(this.SendEncryptedRequest(new SIMCommon.Requests.Get()));
                return response.Messages;
            }
            catch (InvalidResponseException)
            {
                return null;
            }
        }

        public void GetProfiles()
        {
            this.Profiles = JsonConvert.DeserializeObject<SIMCommon.Responses.GetProfiles>(this.SendEncryptedRequest(new SIMCommon.Requests.GetProfiles())).Users;
        }

        public bool InitConnection()
        {
            try
            {
                var request = new SIMCommon.Requests.InitConnection(this.PGPClient.PublicKey);
                var response = TcpClient.Send(this.Address.ToString(), JsonConvert.SerializeObject(request));
                var baseResponse = JsonConvert.DeserializeObject<SIMCommon.Responses.Base>(response);
                if (baseResponse.ResponseType == typeof(SIMCommon.Responses.InitConnection))
                {
                    var convertedResponse = JsonConvert.DeserializeObject<SIMCommon.Responses.InitConnection>(response);
                    this.ServerPublicKey = convertedResponse.PublicKey;
                    this.LeaseDuration = convertedResponse.LeaseDuration;
                    //this.LeaseMonitor.Start();
                    return true;
                }

                return false;
            }
            catch (Exception e) when (e is TcpClient.ConnectionFailiureException || e is Newtonsoft.Json.JsonReaderException)
            {
                return false;
            }
        }

        public bool Register(string username, string password)
        {
            var request = new SIMCommon.Requests.Register(username, password);
            try
            {
                var response = JsonConvert.DeserializeObject<SIMCommon.Responses.Register>(this.SendEncryptedRequest(request));
                return response.Success;
            }
            catch (InvalidResponseException)
            {
                return false;
            }
        }

        public bool Send(SIMCommon.Message message)
        {
            var request = new SIMCommon.Requests.Send(message);
            try
            {
                var response = JsonConvert.DeserializeObject<SIMCommon.Responses.Send>(this.SendEncryptedRequest(request));
                return response.Success;
            }
            catch (InvalidResponseException)
            {
                return false;
            }
        }

        public bool SignIn(string username, string password)
        {
            var signInRequest = new SIMCommon.Requests.SignIn(username, password);
            try
            {
                var response = JsonConvert.DeserializeObject<SIMCommon.Responses.SignIn>(this.SendEncryptedRequest(signInRequest));
                if (response.Success)
                {
                    this.GetProfiles();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (InvalidResponseException)
            {
                return false;
            }
        }

        public void SignOut()
        {
            var request = new SIMCommon.Requests.SignOut();
            this.SendEncryptedRequest(request);
        }

        public SIMCommon.UserProfile UserRef(string username)
        {
            try
            {
                var request = new SIMCommon.Requests.UserRef(username);
                var response = JsonConvert.DeserializeObject<SIMCommon.Responses.UserRef>(this.SendEncryptedRequest(request));
                return response.Profile;
            }
            catch (InvalidResponseException)
            {
                return null;
            }
        }

        private void RunLeaseMonitor()
        {
            System.Threading.Thread.Sleep(SIMCommon.Constants.LeaseMonitorDelay);
            while (true)
            {
                int diff = (int)(DateTime.Now - this.LeaseStart).TotalMilliseconds;
                if (diff < this.LeaseDuration - SIMCommon.Constants.LeaseMonitorDelay)
                {
                    var request = new SIMCommon.Requests.Renew();
                    var responseData = this.SendEncryptedRequest(request);
                    var response = JsonConvert.DeserializeObject<SIMCommon.Responses.Base>(responseData);
                    if (response.ResponseType == typeof(SIMCommon.Responses.Renew))
                    {
                        this.LeaseStart = DateTime.Now;
                    }
                }

                System.Threading.Thread.Sleep(SIMCommon.Constants.LeaseMonitorDelay);
            }
        }

        private string EncryptRequest(string serializedRequest)
        {
            if (!string.IsNullOrEmpty(this.ServerPublicKey))
            {
                string encrypted = this.PGPClient.Encrypt(serializedRequest, this.ServerPublicKey);
                return encrypted;
            }
            else
            {
                return null;
            }
        }

        private string SendEncryptedRequest(SIMCommon.Requests.Base request)
        {
            string encryptedRequestData = this.EncryptRequest(JsonConvert.SerializeObject(request));
            var encryptedRequest = JsonConvert.SerializeObject(new SIMCommon.Requests.Encrypted(this.PGPClient.EncryptedSessionKey, encryptedRequestData));
            string response = TcpClient.Send(this.Address.ToString(), encryptedRequest);
            var encryptedResponse = JsonConvert.DeserializeObject<SIMCommon.Responses.Encrypted>(response);
            string decrypted = this.PGPClient.Decrypt(encryptedResponse.EncryptedResponse, encryptedResponse.EncryptedSessionKey);
            if (decrypted == SIMCommon.Constants.SIMServerInvalidRequestResponse)
            {
                throw new InvalidResponseException();
            }

            return decrypted;
        }

        private string SendRequest(SIMCommon.Requests.Base request)
        {
            string serialized = JsonConvert.SerializeObject(request);
            string response = TcpClient.Send(this.Address.ToString(), serialized);
            if (response == SIMCommon.Constants.SIMServerInvalidRequestResponse)
            {
                throw new InvalidResponseException();
            }

            return response;
        }

        public class InvalidResponseException : Exception
        {
            public InvalidResponseException()
            {
            }
        }
    }
}
