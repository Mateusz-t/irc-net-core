using IrcNetCore.Common;
using Spectre.Console;

namespace IrcNetCoreClient.Commands;

public class ShowChannelUsersCommand : ICommand
{
    private string _channelName;
    public ShowChannelUsersCommand(string channelName)
    {
        _channelName = channelName;
    }
    public string GetCommandRequest()
    {
        return $"{CommandsNames.ShowChannelUsersCommand} {_channelName}";
    }
    public void ProcessResponse(string response)
    {
        if (response.Length < CommandsNames.ShowChannelUsersCommand.Length + 1)
        {
            return;
        }
        response = response.Remove(0, CommandsNames.ShowChannelUsersCommand.Length + 1);
        List<string> usernames = response.Split(";").ToList();
        var columns = usernames.Select(a => new Text(a));
        var rule = new Rule("[underline blue]Users[/]");
        AnsiConsole.Write(rule);
        AnsiConsole.Write(new Columns(columns));
    }
}