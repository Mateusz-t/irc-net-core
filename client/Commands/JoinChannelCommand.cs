using IrcNetCore.Common;
using Spectre.Console;

namespace IrcNetCoreClient.Commands;

public class JoinChannelCommand : ICommand
{
    private string _channelName;
    public JoinChannelCommand(string channelName)
    {
        _channelName = channelName;
    }
    public string GetCommandRequest()
    {
        return $"{CommandsNames.JoinChannelCommand} {_channelName}";
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