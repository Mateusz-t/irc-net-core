using System.Net.Sockets;
using IrcNetCore.Common;

namespace IrcNetCoreClient
{
    public class ClientManager
    {
        private const int PORT = 12345;
        private const string IP_ADDRESS = "127.0.0.1";
        private SocketManager? _socketManager;
        public void Run()
        {
            ConsoleManager.WriteWelcomeMessage();
            if (!CreateSocket())
            {
                return;
            }
            WaitForInitialAck();
            ShowChannelsList();
        }

        private bool CreateSocket()
        {
            _socketManager = new(IP_ADDRESS, PORT);
            try
            {
                _socketManager.Connect();
                return true;
            }
            catch (SocketException)
            {
                ConsoleManager.WriteSocketErrorMessage();
                return false;
            }
        }

        private void WaitForInitialAck(){
            if (_socketManager == null)
                throw new Exception("Socket is not initialized!");
            byte[] buffer = new byte[1024];
            string message = _socketManager.ReceiveMessage(buffer);
            if (message != Commands.ACK_COMMAND)
                throw new Exception("Initial ACK not received!");
            Console.WriteLine("Initial ACK received.");
        }

        private void ShowChannelsList()
        {
            if (_socketManager == null)
                throw new Exception("Socket is not initialized!");
            _socketManager.SendMessage(Commands.SHOW_CHANNELS_LIST_COMMAND);
        }
    }
}