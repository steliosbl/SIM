namespace SIMClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    public static class TcpClient
    {
        public static string Send(string address, string message)
        {
            try
            {
                var client = new System.Net.Sockets.TcpClient(address, SIMCommon.Constants.SIMServerPort);
                var stream = client.GetStream();

                byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);

                data = new byte[SIMCommon.Constants.TcpStreamByteSize];
                string response = string.Empty;
                int bytes = stream.Read(data, 0, data.Length);
                response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                stream.Close();
                client.Close();

                return response;
            }
            catch (Exception e) when (e is SocketException || e is System.IO.IOException)
            {
                throw new ConnectionFailiureException();
            }
        }

        public class ConnectionFailiureException : Exception
        {
            public ConnectionFailiureException()
            {
            }
        }
    }
}
