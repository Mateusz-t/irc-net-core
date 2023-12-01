using IrcNetCore.Common;
using Spectre.Console;

namespace IrcNetCoreClient.Commands;

public class ShowChannelUsersCommand : ICommand
{
    public string GetCommandRequest(string parameters)
    {
        return CommandsNames.ShowChannelUsersCommand;
    }

    public void ProcessResponse(string response)
    {
        List<string> usernames = response.Split(";").ToList();
        var columns = usernames.Select(a => new Text(a));
        var rule = new Rule("[underline blue]Users[/]");
        AnsiConsole.Write(rule);
        AnsiConsole.Write(new Columns(columns));
    }
}