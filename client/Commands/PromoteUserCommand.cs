using IrcNetCore.Common;
using Spectre.Console;

namespace IrcNetCoreClient.Commands;

public class PromoteUserCommand : ICommand
{
    private string _channelName;
    private string _username;
    public PromoteUserCommand(string channelName, string username)
    {
        _channelName = channelName;
        _username = username;
    }

    public string GetCommandRequest()
    {
        return $"{CommandsNames.PromoteUserCommand} {_channelName} {_username}";
    }

    public void ProcessResponse(string response)
    {
        if (response.Length < CommandsNames.PromoteUserCommand.Length + 1)
        {
            AnsiConsole.Markup($"[red]Could not promote user![/]\n");
            return;
        }
        response = response.Remove(0, CommandsNames.PromoteUserCommand.Length + 1);
        AnsiConsole.Markup($"[green]User {_username} promoted to[/] [yellow]{response}[/][green]![/]\n");
    }
}