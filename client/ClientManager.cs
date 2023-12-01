using System.Net.Sockets;
using IrcNetCore.Common;
using IrcNetCoreClient.Commands;

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
            Login();
            ConsoleManager.WriteWelcomeMessage();
            StartMenuLoop();
        }

        private void StartMenuLoop()
        {
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
            if (message != CommandsNames.AckCommand)
                throw new Exception("Initial ACK not received!");
            ConsoleManager.WriteInfoMessage("Initial ACK received.");
        }

        private bool ShowMenu()
        {
            //create a map of commands and their handlers
            var commands = new Dictionary<string, Action>
            {
                {ShowChannelListCommand.GetMenuText(), ShowChannelsList},
                {JoinChannelCommand.GetMenuText(), JoinChannel}
            };
            string selected = ConsoleManager.WriteMenuAndGetSelectedOption(commands.Keys);

            if (selected == "Exit")
            {
                return false;
            }
            else if (commands.TryGetValue(selected, out var action))
            {
                action();
                return true;
            }
            else
            {
                ConsoleManager.WriteErrorMessage("Invalid option!");
                return false;
            }
        }

        private void Login()
        {
            string username = ConsoleManager.AskForUsername();
            LoginCommand loginCommand = new();
            SendCommand(loginCommand, username);
        }

        private void JoinChannel()
        {
            string channelName = ConsoleManager.AskForChannelName();
            JoinChannelCommand joinChannelCommand = new();
            SendCommand(joinChannelCommand, channelName);
            StartChannelLoop(channelName);
        }

        private void StartChannelLoop(string channelName)
        {
            ConsoleManager.WriteChannelMessage(channelName);
            // Thread thread = new(WaitForMessage);
            // thread.Start();
            while (ShowChannelMenu(channelName)) ;
        }

        private void WaitForMessage()
        {
            if (_socketManager == null)
                throw new Exception("Socket is not initialized!");
            while (true)
            {
                string message = _socketManager.ReceiveMessage();
                Console.WriteLine(message);
            }
        }

        private bool ShowChannelMenu(string channelName)
        {
            string message = ConsoleManager.AskForMessage();
            if (message == "/close")
            {
                return false;
            }
            else if (message == "/exit")
            {
                ExitChannelCommand exitChannelCommand = new();
                SendCommand(exitChannelCommand, channelName);
                return false;
            }
            else if (message == "/help")
            {
                ConsoleManager.ShowChannelHelp();
                return true;
            }
            else
            {
                SendMessageCommand sendMessageCommand = new();
                SendCommand(sendMessageCommand, $"{channelName} {message}");
                return true;
            }
        }
        private void ShowChannelsList()
        {
            ShowChannelListCommand channelListCommand = new();
            SendCommand(channelListCommand);
        }

        private void SendCommand(ICommand command, string? message = null)
        {
            if (_socketManager == null)
                throw new Exception("Socket is not initialized!");
            _socketManager.SendMessage(command.GetCommandRequest(message ?? string.Empty));
            string response = _socketManager.ReceiveMessage();
            command.ProcessResponse(response);
        }
    }
}