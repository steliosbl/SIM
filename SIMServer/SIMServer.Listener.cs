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

        public Listener(int port, ReceivedDataHandler dataHandler = null)
        {
            this.client = null;
            this.port = port;
            this.server = null;
            this.stream = null;

            this.ReceivedData += dataHandler;
        }

        public delegate void ReceivedDataHandler(EventArgs e, string data, IPAddress address);

        public event ReceivedDataHandler ReceivedData;

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

        protected virtual void OnReceivedData(EventArgs e, string data, IPAddress address)
        {
            if (this.ReceivedData != null)
            {
                this.ReceivedData(e, data, address);
            }
        }
    }
}