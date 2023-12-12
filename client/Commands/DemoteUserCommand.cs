using IrcNetCore.Common;
using Spectre.Console;

namespace IrcNetCoreClient.Commands;

public class DemoteUserCommand : ICommand
{
    private string _channelName;
    private string _username;
    public DemoteUserCommand(string channelName, string username)
    {
        _channelName = channelName;
        _username = username;
    }

    public string GetCommandRequest()
    {
        return $"{CommandsNames.DemoteUserCommand} {_channelName} {_username}";
    }

    public void ProcessResponse(string response)
    {
        if (response.Length < CommandsNames.DemoteUserCommand.Length + 1)
        {
            AnsiConsole.Markup($"[red]Could not demote user![/]\n");
            return;
        }
        response = response.Remove(0, CommandsNames.DemoteUserCommand.Length + 1);
        AnsiConsole.Markup($"[green]User {_username} demoted to[/] [red]{response}[/][green]![/]\n");
    }
}