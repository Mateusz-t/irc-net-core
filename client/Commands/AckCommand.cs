using IrcNetCore.Common;

namespace IrcNetCoreClient.Commands;

public class AckCommand : ICommand
{
    public string GetCommandRequest(string parameters)
    {
        return CommandsNames.AckCommand;
    }

    public void ProcessResponse(string response)
    {
        return;
    }
}