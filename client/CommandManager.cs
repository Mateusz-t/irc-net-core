using IrcNetCoreClient.Commands;

namespace IrcNetCoreClient;

public class CommandManager
{
    private SocketManager _socketManager;

    public CommandManager(SocketManager socketManager)
    {
        _socketManager = socketManager;

    }


    public void SendCommandAndProcess(ICommand command)
    {
        SendCommand(command);
        string response = _socketManager.ReceiveMessage();
        command.ProcessResponse(response);
    }

    public void SendCommand(ICommand command)
    {
        _socketManager.SendMessage(command.GetCommandRequest());
    }
}
