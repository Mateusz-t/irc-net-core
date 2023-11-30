namespace IrcNetCore.Common.Commands;

public class AckCommand : ICommand
{
    public const string COMMAND = "/ack";
    public string GetCommandToSend(string message)
    {
        return COMMAND;
    }

    public string GetCommandResponse(string message)
    {
        return COMMAND;
    }

    public void ProcessCommand(string message)
    {
        return;
    }
}