using IrcNetCore.Common;
using Spectre.Console;

namespace IrcNetCoreClient.Commands;

public class JoinChannelCommand : ICommand
{
    private readonly string _channelName;
    public bool Result { get; private set; }
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
        if (response.Length < CommandsNames.JoinChannelCommand.Length + 1)
        {
            AnsiConsole.Markup($"[red]Error joining channel {response}[/]\n");
            Result = false;
            return;
        }
        response = response.Remove(0, CommandsNames.JoinChannelCommand.Length + 1);
        AnsiConsole.Markup($"[green]Joined channel {response}[/]\n");
        Result = true;
    }

    public static string GetMenuText()
    {
        return "Join channel";
    }
}