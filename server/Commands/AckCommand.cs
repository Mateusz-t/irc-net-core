using IrcNetCore.Common;

namespace IrcNetCoreServer.Commands;

public class AckCommand : ICommand
{
    public string GetCommandResponse(string parameters)
    {
        return CommandsNames.AckCommand;
    }

    public void ProcessCommand(string parameters)
    {
        return;
    }
}
