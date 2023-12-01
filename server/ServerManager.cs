using System.Text;
using IrcNetCoreServer.Commands;
using IrcNetCoreServer.Events;

namespace IrcNetCoreServer;

public class ServerManager
{
    private const int PORT = 12345;
    private const string IP_ADDRESS = "127.0.0.1";
    private SocketListener? _socketListener;
    private ChannelManager _channelManager = new();

    public void Run()
    {
        _socketListener = new(IP_ADDRESS, PORT);
        _socketListener.Start();
        _socketListener.OnCommandReceived += ProcessCommand;
    }

    public void ProcessCommand(object? sender, CommandReceivedEventArgs e)
    {
        string response = GetCommandResponse(e);
        byte[] encodedCommand = Encoding.ASCII.GetBytes(response);
        e.ClientSocket.Send(encodedCommand);
        Console.WriteLine($"Send {e.ClientSocket.RemoteEndPoint}: {response}");
    }

    public string GetCommandResponse(CommandReceivedEventArgs e)
    {
        if (_socketListener == null)
            throw new Exception("Socket listener is not initialized!");

        List<string> splittedCommand = e.Command.Split(' ').ToList();
        string commandName = splittedCommand[0];
        string commandParameters = string.Join(" ", splittedCommand.Skip(1));

        CommandFactory commandFactory = new(_channelManager, e);
        ICommand command = commandFactory.GetCommand(commandName);
        command.ProcessCommand(commandParameters);
        return command.GetCommandResponse(commandParameters);
    }
}
