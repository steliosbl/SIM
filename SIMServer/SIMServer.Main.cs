namespace SIMServer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class Main
    {
        public Main()
        {
            var configInterface = new SIMCommon.JsonFileInterface(SIMCommon.Constants.SIMServerConfigFilename);
            this.Config = configInterface.GetObject<Config>();

            this.Clients = new Dictionary<IPAddress, SIMServer.Client>();

            this.Listener = new SIMServer.Listener(SIMCommon.Constants.SIMServerPort, this.ProcessRequest);

            this.Database = new SIMServer.AuthDB(this.Config.ConnectionInfo);

            if (!File.Exists(SIMCommon.Constants.SIMServerBacklogFilename) || File.ReadAllText(SIMCommon.Constants.SIMServerBacklogFilename).Trim().Length == 0)
            {
                this.Backlog = new Backlog();
            }
            else
            {
                var backlogInterface = new SIMCommon.JsonFileInterface(SIMCommon.Constants.SIMServerBacklogFilename);
                this.Backlog = backlogInterface.GetObject<Backlog>();
            }

            this.Logger = new SIMServer.Logger(SIMCommon.Constants.SIMServerLoggerFilename, true);

            this.LeaseMonitor = new Thread(() => this.MonitorLeases());
            this.LeaseMonitor.Start();

            this.Listener.Start();
        }

        public Dictionary<IPAddress, Client> Clients { get; private set; }

        public Listener Listener { get; private set; }

        public Config Config { get; private set; }

        public AuthDB Database { get; private set; }

        public Backlog Backlog { get; private set; }

        public Thread LeaseMonitor { get; private set; }

        public Logger Logger { get; private set; }

        private void ProcessRequest(EventArgs e, string data, IPAddress address)
        {
            string response = null;
            bool encrypt = this.Clients.ContainsKey(address);
            try
            {
                if (encrypt)
                {
                    var encryptedRequest = JsonConvert.DeserializeObject<SIMCommon.Requests.Encrypted>(data);
                    data = this.Clients[address].PGPClient.Decrypt(encryptedRequest.EncryptedRequest, encryptedRequest.EncryptedSessionKey);
                }

                var baseRequest = JsonConvert.DeserializeObject<SIMCommon.Requests.Base>(data);
                this.Logger.Log("[" + address.ToString() + "] " + "REQUEST RECEIVED {" + baseRequest.RequestType.Name + "}");
                dynamic request = JsonConvert.DeserializeObject(data, baseRequest.RequestType);
                response = this.RequestHandler(address, request);
            }
            catch (JsonException)
            {
                this.Logger.Log("[" + address.ToString() + "] " + "INVALID REQUEST RECEIVED");
                response = SIMCommon.Constants.SIMServerInvalidRequestResponse;
            }
            finally
            {
                if (encrypt)
                {
                    string encryptedResponse = this.Clients[address].PGPClient.Encrypt(response, this.Clients[address].PublicKey);
                    response = JsonConvert.SerializeObject(new SIMCommon.Responses.Encrypted(this.Clients[address].PGPClient.EncryptedSessionKey, encryptedResponse));
                }

                this.Listener.Respond(response);
            }
        }

        private string RequestHandler(IPAddress address, SIMCommon.Requests.Base request)
        {
            return SIMCommon.Constants.SIMServerInvalidRequestResponse;
        }

        private string RequestHandler(IPAddress address, SIMCommon.Requests.Authenticate request)
        {
            SIMCommon.Responses.Authenticate result;
            if (this.Database.UserExists(request.Username))
            {
                result = new SIMCommon.Responses.Authenticate(this.Database.GetUser(this.Database.GetUserID(request.Username)).Authenticate(request.Password));
            }
            else
            {
                result = new SIMCommon.Responses.Authenticate(false);
            }

           return JsonConvert.SerializeObject(result);
        }

        private string RequestHandler(IPAddress address, SIMCommon.Requests.EndConnection request)
        {
            this.Clients.Remove(address);
            var result = new SIMCommon.Responses.EndConnection();
            return JsonConvert.SerializeObject(result);
        }

        private string RequestHandler(IPAddress address, SIMCommon.Requests.Get request)
        {
            if (this.Clients[address].User != null)
            {
                var messages = new List<SIMCommon.Message>();
                if (this.Backlog.Messages.ContainsKey(this.Clients[address].User.ID))
                {
                    messages = this.Backlog.Messages[this.Clients[address].User.ID];
                }

                var result = new SIMCommon.Responses.Get(messages);
                return JsonConvert.SerializeObject(result);
            }
            else
            {
                return JsonConvert.SerializeObject(new SIMCommon.Responses.Get(null));
            }
        }

        private string RequestHandler(IPAddress address, SIMCommon.Requests.GetProfiles request)
        {
            var profs = new Dictionary<int, SIMCommon.UserProfile>();
            foreach (var client in this.Clients.Values.ToList().FindAll(client => client.User != null))
            {
                profs.Add(client.User.ID, new SIMCommon.UserProfile(client.User.ID, client.User.Nickname, true));
            }

            var allProfiles = this.Database.GetProfiles();
            foreach (var profile in allProfiles.FindAll(profile => !profs.ContainsKey(profile.ID)))
            {
                profs.Add(profile.ID, profile);
            }

            var result = new SIMCommon.Responses.GetProfiles(profs.Values.ToList());
            return JsonConvert.SerializeObject(result);
        }

        private string RequestHandler(IPAddress address, SIMCommon.Requests.InitConnection request)
        {
            SIMCommon.Responses.InitConnection response;
            if (!this.Clients.ContainsKey(address))
            {
                var newClient = new Client(address, request.PublicKey);
                this.Clients.Add(address, newClient);
                response = new SIMCommon.Responses.InitConnection(true, newClient.PGPClient.PublicKey, this.Config.LeaseDuration);
            }
            else
            {
                response = new SIMCommon.Responses.InitConnection(false);
            }

            return JsonConvert.SerializeObject(response);
        }

        private string RequestHandler(IPAddress address, SIMCommon.Requests.Register request)
        {
            SIMCommon.Responses.Register response = null;
            if (!this.Database.UserExists(request.Username))
            {
                var newUser = new User(this.Database.GetLastUserID() + 1, request.Username, request.Password);
                this.Database.AddUser(newUser);
                response = new SIMCommon.Responses.Register(true);
            }
            else
            {
                response = new SIMCommon.Responses.Register(false);
            }

            return JsonConvert.SerializeObject(response);
        }

        private string RequestHandler(IPAddress address, SIMCommon.Requests.Ping request)
        {
            return JsonConvert.SerializeObject(new SIMCommon.Responses.Ping(true));
        }

        private string RequestHandler(IPAddress address, SIMCommon.Requests.Renew request)
        {
            if (this.Clients[address].RemainingLeaseTime(this.Config.LeaseDuration) <= SIMCommon.Constants.LeaseMonitorDelay)
            {
                this.Clients[address].RenewLease();
                return JsonConvert.SerializeObject(new SIMCommon.Responses.Renew());
            }
            else
            {
                return SIMCommon.Constants.SIMServerInvalidRequestResponse;
            }
        }

        private string RequestHandler(IPAddress address, SIMCommon.Requests.Send request)
        {
            SIMCommon.Responses.Send result;
            if (this.Database.UserExists(request.Message.RecipientID))
            {
                this.Backlog.Messages[request.Message.RecipientID].Add(request.Message);
                result = new SIMCommon.Responses.Send(true);
            }
            else
            {
                result = new SIMCommon.Responses.Send(false);
            }

            return JsonConvert.SerializeObject(result);
        }

        private string RequestHandler(IPAddress address, SIMCommon.Requests.SignIn request)
        { 
            SIMCommon.Responses.SignIn result;
            if (this.Database.UserExists(request.Username))
            {
                var user = this.Database.GetUser(this.Database.GetUserID(request.Username));
                if (user.Authenticate(request.Password))
                {
                    this.Clients[address].LoadUser(user);
                    result = new SIMCommon.Responses.SignIn(true);
                }
                else
                {
                    result = new SIMCommon.Responses.SignIn(false);
                }
            }
            else
            {
                result = new SIMCommon.Responses.SignIn(false);
            }

            return JsonConvert.SerializeObject(result);
        }

        private string RequestHandler(IPAddress address, SIMCommon.Requests.SignOut request)
        {
            this.Clients[address].RemoveUser();
            var result = new SIMCommon.Responses.SignOut();
            return JsonConvert.SerializeObject(result);
        }

        private string RequestHandler(IPAddress address, SIMCommon.Requests.UserRef request)
        {
            SIMCommon.Responses.UserRef result;
            if (this.Database.UserExists(request.Username))
            {
                var user = this.Database.GetUser(this.Database.GetUserID(request.Username));
                result = new SIMCommon.Responses.UserRef(new SIMCommon.UserProfile(user.ID, user.Nickname));
            }
            else
            {
                result = new SIMCommon.Responses.UserRef(null);
            }

            return JsonConvert.SerializeObject(result);
        }

        private bool CheckUserActive(int id)
        {
            if (this.Clients.Values.ToList().Count(client => client.User.ID == id) == 1)
            {
                return true;
            }

            return false;
        }

        private IPAddress GetUserClient(int id)
        {
            if (this.CheckUserActive(id))
            {
                return this.Clients.Keys.ToList().Find(client => this.Clients[client].User.ID == id);
            }

            return null;
        }

        private void MonitorLeases()
        {
            while (true)
            {
                foreach (var client in this.Clients.Keys.ToList())
                {
                    if (this.Clients[client].CheckLeaseExpired(this.Config.LeaseDuration))
                    {
                        this.Clients.Remove(client);
                    }
                }

                Thread.Sleep(SIMCommon.Constants.LeaseMonitorDelay);
            }
        }
    }
}