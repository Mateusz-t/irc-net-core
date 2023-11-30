using System.Text;
using IrcNetCore.Common.Commands;
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
        string response = GetCommandResponse(e.Command);
        byte[] encodedCommand = Encoding.ASCII.GetBytes(response);
        e.ClientSocket.Send(encodedCommand);
        Console.WriteLine($"Send {e.ClientSocket.RemoteEndPoint}: {e.Command} {response}");
    }

    public string GetCommandResponse(string commandMessage)
    {
        if (_socketListener == null)
            throw new Exception("Socket listener is not initialized!");

        CommandFactory commandFactory = new(_channelManager.GetChannelsList());
        ICommand command = commandFactory.GetCommand(commandMessage);
        return command.GetCommandResponse(commandMessage);
    }
}
