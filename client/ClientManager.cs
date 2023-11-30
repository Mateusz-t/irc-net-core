using System.Net.Sockets;
using IrcNetCore.Common;
using IrcNetCore.Common.Commands;

namespace IrcNetCoreClient
{
    public class ClientManager
    {
        private const int PORT = 12345;
        private const string IP_ADDRESS = "127.0.0.1";
        private SocketManager? _socketManager;
        public void Run()
        {
            try
            {
                RunInternal();
            }
            catch (Exception e)
            {
                ConsoleManager.WriteErrorMessage(e.Message);
            }
        }

        private void RunInternal()
        {
            ConsoleManager.WriteInitializationMessage();
            if (!CreateSocket())
            {
                return;
            }
            WaitForInitialAck();
            ConsoleManager.WriteWelcomeMessage();
            while (ShowMenu()) ;
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
                ConsoleManager.WriteErrorMessage("Could not connect to a server!");
                return false;
            }
        }

        private void WaitForInitialAck()
        {
            if (_socketManager == null)
                throw new Exception("Socket is not initialized!");
            string message = _socketManager.ReceiveMessage();
            if (message != AckCommand.COMMAND)
                throw new Exception("Initial ACK not received!");
            ConsoleManager.WriteInfoMessage("Initial ACK received.");
        }

        private bool ShowMenu()
        {
            string selected = ConsoleManager.WriteMenuAndGetSelectedOption();

            switch (selected)
            {
                case ChannelListCommand.MenuText:
                    ShowChannelsList();
                    return true;
                default:
                    ConsoleManager.WriteErrorMessage("Invalid option!");
                    return false;
            }
        }

        private void ShowChannelsList()
        {
            ChannelListCommand channelListCommand = new();
            SendCommand(channelListCommand);
        }

        private void SendCommand(ICommand command, string? message = null)
        {
            if (_socketManager == null)
                throw new Exception("Socket is not initialized!");
            _socketManager.SendMessage(command.GetCommandToSend(message ?? string.Empty));
            string response = _socketManager.ReceiveMessage();
            command.ProcessCommand(response);
        }
    }
}