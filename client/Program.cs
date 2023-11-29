using System.Net;
using System.Net.Sockets;
using System.Text;

namespace IrcNetCoreClient
{
    class Program
    {
        private const int PORT = 12345;
        private const string IP_ADDRESS = "127.0.0.1";
        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse(IP_ADDRESS);
            IPEndPoint iPEndPoint = new(ipAddress, PORT);

            using Socket client = new(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(iPEndPoint);

            while (true)
            {
                string message = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                client.Send(buffer);
            }
            // client.Shutdown(SocketShutdown.Both);
        }
    }
}