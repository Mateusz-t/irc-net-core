using IrcNetCore.Common;
using Spectre.Console;

namespace IrcNetCoreClient.Commands;

public class SendMessageCommand : ICommand
{
    public string GetCommandRequest(string parameters)
    {
        return $"{CommandsNames.SendMessageCommand} {parameters}";
    }

    public void ProcessResponse(string response)
    {
        AnsiConsole.Markup($"[green]{response}[/]\n");
    }
}