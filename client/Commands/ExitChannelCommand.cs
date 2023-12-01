using IrcNetCore.Common;
using Spectre.Console;

namespace IrcNetCoreClient.Commands;

public class ExitChannelCommand : ICommand
{
    public string GetCommandRequest(string channelName)
    {
        return $"{CommandsNames.ExitChannelCommand} {channelName}";
    }

    public void ProcessResponse(string response)
    {
        AnsiConsole.Markup($"[green]Left channel {response}[/]\n");

    }
}