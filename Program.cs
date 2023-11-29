using System.Net;
using System.Net.Sockets;
using System.Text;

namespace IrcNetCore.Server
{
    class Program
    {
        private const int PORT = 12345;
        private const string IP_ADDRESS = "127.0.0.1";

        static void Main(string[] args)
        {
            SocketListener listener = new(IP_ADDRESS, PORT);
            listener.Start();
        }
    }
}