using IrcNetCore.Common;
using Spectre.Console;

namespace IrcNetCoreClient.Commands;

public class NotifyChannelUsersCommand : ICommand
{
    public string GetCommandRequest()
    {
        return CommandsNames.NotifyChannelUsersCommand;
    }

    public void ProcessResponse(string response)
    {
        if (response.Length < CommandsNames.NotifyChannelUsersCommand.Length + 1)
        {
            return;
        }
        response = response.Remove(0, CommandsNames.NotifyChannelUsersCommand.Length + 1);

        AnsiConsole.Markup($"[green]{response}[/]\n");
    }
}