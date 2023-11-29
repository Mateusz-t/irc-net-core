namespace IrcNetCoreServer
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