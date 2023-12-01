using IrcNetCore.Common;
using Spectre.Console;

namespace IrcNetCoreClient.Commands;

public class NotifyChannelUsersCommand : ICommand
{
    public string GetCommandRequest(string parameters)
    {
        return CommandsNames.NotifyChannelUsersCommand;
    }

    public void ProcessResponse(string response)
    {
        AnsiConsole.Markup($"{response}\n");
    }
}