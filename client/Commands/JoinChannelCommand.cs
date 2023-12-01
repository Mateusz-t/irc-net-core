using IrcNetCore.Common;
using Spectre.Console;

namespace IrcNetCoreClient.Commands;

public class JoinChannelCommand : ICommand
{
    public string GetCommandRequest(string channelName)
    {
        return $"{CommandsNames.JoinChannelCommand} {channelName}";
    }

    public void ProcessResponse(string response)
    {
        AnsiConsole.Markup($"[green]Joined channel {response}[/]\n");

    }

    public static string GetMenuText()
    {
        return "Join channel";
    }
}