using IrcNetCore.Common;
using Spectre.Console;

namespace IrcNetCoreClient.Commands;

public class ExitChannelCommand : ICommand
{
    private string _channelName;
    public ExitChannelCommand(string channelName)
    {
        _channelName = channelName;
    }
    public string GetCommandRequest()
    {
        return $"{CommandsNames.ExitChannelCommand} {_channelName}";
    }

    public void ProcessResponse(string response)
    {
        AnsiConsole.Markup($"[green]Left channel {response}[/]\n");

    }
}