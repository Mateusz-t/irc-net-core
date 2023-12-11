using System.Text;
using IrcNetCoreServer.Commands;
using IrcNetCoreServer.Events;

namespace IrcNetCoreServer;

public class ServerManager
{
    private const int COMMAND_PORT = 50000;
    private const int MESSAGE_PORT = 50001;
    private const string IP_ADDRESS = "127.0.0.1";
    private SocketListener _commandSocketListener;
    private SocketListener _messageSocketListener;
    private ChannelManager _channelManager = new();

    public ServerManager()
    {
        _commandSocketListener = new("CommandSocket", IP_ADDRESS, COMMAND_PORT);
        _messageSocketListener = new("MessageSocket", IP_ADDRESS, MESSAGE_PORT);
    }
    public void Run()
    {
        _commandSocketListener.Start(true);
        _commandSocketListener.OnCommandReceived += ProcessCommand;
        _messageSocketListener.Start();
        _messageSocketListener.OnCommandReceived += ProcessMessage;
    }

    public void ProcessCommand(object? sender, CommandReceivedEventArgs e)
    {
        string response = GetCommandResponse(e);
        byte[] encodedCommand = Encoding.ASCII.GetBytes(response);
        e.ClientSocket.Send(encodedCommand);
        Console.WriteLine($"(CommandSocket) Send {e.ClientSocket.RemoteEndPoint}: {response}");
    }

    public void ProcessMessage(object? sender, CommandReceivedEventArgs e)
    {
        List<string> splittedCommand = e.Command.Split(' ').ToList();
        string channelName = splittedCommand[1];
        _channelManager.StartListeningToChannel(channelName, e.ClientSocket);
        Console.WriteLine($"(MessageSocket) Client {e.ClientSocket.RemoteEndPoint} is listening for messages");
    }

    public string GetCommandResponse(CommandReceivedEventArgs e)
    {
        List<string> splittedCommand = e.Command.Split(' ').ToList();
        string commandName = splittedCommand[0];
        string commandParameters = string.Join(" ", splittedCommand.Skip(1));

        CommandFactory commandFactory = new(_channelManager, e);
        ICommand command = commandFactory.GetCommand(commandName);
        command.ProcessCommand(commandParameters);
        return command.GetCommandResponse(commandParameters);
    }
}
