using System.Net;
using System.Net.Sockets;
using System.Text;

namespace IrcNetCoreClient
{
    public class SocketManager
    {
        private readonly string _IpAddress;
        private readonly int _Port;
        private Socket? _clientSocket;
        public SocketManager(string ipAddress, int port)
        {
            _IpAddress = ipAddress;
            _Port = port;
        }

        public void Connect()
        {
            _clientSocket = CreateSocket();
            _clientSocket.Connect(_IpAddress, _Port);
        }

        public void SendMessage(string message)
        {
            if (_clientSocket == null)
                throw new Exception("Socket is not initialized!");

            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            _clientSocket.Send(messageBytes);
            //temp
            Console.WriteLine($"Message sent: {message}");
        }

        public string ReceiveMessage(byte[] buffer)
        {
            if (_clientSocket == null)
                throw new Exception("Socket is not initialized!");

            int receivedBytes = _clientSocket.Receive(buffer);
            string message = Encoding.ASCII.GetString(buffer, 0, receivedBytes).Trim();
            //temp
            Console.WriteLine($"Message received: {message}");
            return message;
        }

        private Socket CreateSocket()
        {
            Console.WriteLine("Creating socket...");
            IPAddress ipAddress = IPAddress.Parse(_IpAddress);
            IPEndPoint iPEndPoint = new(ipAddress, _Port);
            Socket clientSocket = new(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Socket created.");
            return clientSocket;
        }


    }
}
