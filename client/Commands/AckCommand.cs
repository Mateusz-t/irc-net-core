using IrcNetCore.Common;

namespace IrcNetCoreClient.Commands;

public class AckCommand : ICommand
{
    public string GetCommandRequest()
    {
        return CommandsNames.AckCommand;
    }

    public void ProcessResponse(string response)
    {
        return;
    }
}