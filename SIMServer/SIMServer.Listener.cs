namespace SIMServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class Listener
    {
        private TcpClient client;
        private int port;
        private TcpListener server;
        private NetworkStream stream;
        private List<IPAddress> clients;

        public Listener(int port, ReceivedClientRequestHandler clientRequestHandler, ReceivedUnknownRequestHandler unknownRequesthandler, ReceivedDataHandler dataHandler = null)
        {
            this.client = null;
            this.port = port;
            this.server = null;
            this.stream = null;
            this.clients = new List<IPAddress>();

            this.ReceivedClientRequest += clientRequestHandler;
            this.ReceivedUnknownRequest += unknownRequesthandler;

            if (dataHandler != null)
            {
                this.ReceivedData += dataHandler;
            }
            else
            {
                this.ReceivedData += this.ReceivedDataSifter;
            }
        }

        public delegate void ReceivedDataHandler(EventArgs e, string data, IPAddress address);

        public delegate void ReceivedClientRequestHandler(EventArgs e, string request, IPAddress address);

        public delegate void ReceivedUnknownRequestHandler(EventArgs e, string request, IPAddress address);

        public event ReceivedDataHandler ReceivedData;

        public event ReceivedClientRequestHandler ReceivedClientRequest;

        public event ReceivedUnknownRequestHandler ReceivedUnknownRequest;

        public void Start()
        {
            this.server = new TcpListener(IPAddress.Any, this.port);
            this.server.Start();

            byte[] bytes = new byte[SIMCommon.Constants.TcpStreamByteSize];
            string data = null;

            while (true)
            {
                try
                {
                    this.client = this.server.AcceptTcpClient();
                    data = null;
                    this.stream = this.client.GetStream();

                    int i;

                    while ((i = this.stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        IPAddress address = IPAddress.Parse(((IPEndPoint)this.client.Client.RemoteEndPoint).Address.ToString());
                        this.OnReceivedData(EventArgs.Empty, data, address);
                    }
                }
                catch (SocketException)
                {
                }
            }
        }

        public void Respond(string response)
        {
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);
            this.stream.Write(msg, 0, msg.Length);
        }

        public void UpdateClients(List<IPAddress> newClients)
        {
            this.clients = newClients;
        }

        protected virtual void OnReceivedData(EventArgs e, string data, IPAddress address)
        {
            if (this.ReceivedData != null)
            {
                this.ReceivedData(e, data, address);
            }
        }

        protected virtual void OnReceivedClientRequest(EventArgs e, string request, IPAddress address)
        {
            if (this.ReceivedClientRequest != null)
            {
                this.ReceivedClientRequest(e, request, address);
            }
        }

        protected virtual void OnReceivedUnknownRequest(EventArgs e, string request, IPAddress address)
        {
            if (this.ReceivedUnknownRequest != null)
            {
                this.ReceivedUnknownRequest(e, request, address);
            }
        }

        private void ReceivedDataSifter(EventArgs e, string data, IPAddress address)
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.MissingMemberHandling = MissingMemberHandling.Error;

                if (this.clients.Contains(address))
                {
                    var request = JsonConvert.DeserializeObject<SIMCommon.Requests.Encrypted>(data, settings);
                    this.OnReceivedClientRequest(e, data, address);
                }
                else
                {
                    var request = JsonConvert.DeserializeObject<SIMCommon.Requests.Base>(data, settings);
                    this.OnReceivedUnknownRequest(e, data, address);
                }
            }
            catch (JsonException)
            {
                this.Respond(SIMCommon.Constants.SIMServerInvalidRequestResponse);
            }
        }
    }
}