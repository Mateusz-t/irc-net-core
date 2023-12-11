using IrcNetCore.Common;
using IrcNetCoreClient.Commands;

namespace IrcNetCoreClient;

public class ClientManager
{
    private const int COMMAND_PORT = 50000;
    private const int MESSAGE_PORT = 50001;
    private const string IP_ADDRESS = "127.0.0.1";
    private SocketManager _commandSocketManager;
    private CommandManager _commandManager;

    public ClientManager()
    {
        ConsoleManager.WriteInitializationMessage();
        _commandSocketManager = new(IP_ADDRESS, COMMAND_PORT);
        _commandSocketManager.Connect();
        _commandManager = new(_commandSocketManager);
    }

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
        WaitForInitialAck();
        Login();
        ConsoleManager.WriteWelcomeMessage();
        StartMenuLoop();
    }

    private void StartMenuLoop()
    {
        while (ShowMenu()) ;
    }
    private void WaitForInitialAck()
    {
        if (_commandSocketManager == null)
            throw new Exception("Socket is not initialized!");
        string message = _commandSocketManager.ReceiveMessage();
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
        _commandManager.SendCommandAndProcess(new LoginCommand(username));
    }

    private void JoinChannel()
    {
        string channelName = ConsoleManager.AskForChannelName();
        var joinChannelCommand = new JoinChannelCommand(channelName);
        _commandManager.SendCommandAndProcess(joinChannelCommand);
        if (joinChannelCommand.Result)
        {
            StartChannelLoop(channelName);
        }
    }

    private void StartChannelLoop(string channelName)
    {
        ConsoleManager.WriteChannelMessage(channelName);
        _commandManager.SendCommandAndProcess(new ShowChannelMessagesCommand(channelName));
        SocketManager messageSocketManager = new(IP_ADDRESS, MESSAGE_PORT);
        messageSocketManager.Connect();
        CommandManager messageCommandManager = new(messageSocketManager);
        messageCommandManager.SendCommand(new JoinChannelCommand(channelName));
        var tokenSource = new CancellationTokenSource();
        CancellationToken token = tokenSource.Token;
        Task task = Task.Run(() => ReceiveMessages(messageSocketManager), token);
        while (ShowChannelMenu(channelName)) ;
        tokenSource.Cancel();
    }

    private void ReceiveMessages(SocketManager socketManager)
    {
        while (true)
        {
            string message = socketManager.ReceiveMessage();
            NotifyChannelUsersCommand notifyChannelUsersCommand = new();
            notifyChannelUsersCommand.ProcessResponse(message);
        }
    }


    private bool ShowChannelMenu(string channelName)
    {
        string message = ConsoleManager.AskForMessage();
        if (message == "/users")
        {
            _commandManager.SendCommandAndProcess(new ShowChannelUsersCommand(channelName));
            return true;
        }
        else if (message.StartsWith("/promote"))
        {
            if (message.Length < 10)
            {
                ConsoleManager.WriteErrorMessage("Invalid command!");
                return true;
            }
            string username = message.Remove(0, 9);
            _commandManager.SendCommandAndProcess(new PromoteUserCommand(channelName, username));
            return true;
        }
        else if (message == "/close")
        {
            return false;
        }
        else if (message == "/exit")
        {
            _commandManager.SendCommandAndProcess(new ExitChannelCommand(channelName));
            return false;
        }
        else if (message == "/help")
        {
            ConsoleManager.ShowChannelHelp();
            return true;
        }
        else
        {
            _commandManager.SendCommandAndProcess(new SendMessageCommand(channelName, message));
            return true;
        }
    }

    private void ShowChannelsList()
    {
        _commandManager.SendCommandAndProcess(new ShowChannelListCommand());
    }
}
