using IrcNetCore.Common;
using Spectre.Console;

namespace IrcNetCoreClient.Commands;

public class ShowChannelListCommand : ICommand
{
    public static string GetMenuText()
    {
        return "Show channels";
    }

    public string GetCommandRequest()
    {
        return CommandsNames.ShowChannelListCommand;
    }

    public void ProcessResponse(string response)
    {
        if (response.Length < CommandsNames.ShowChannelListCommand.Length + 1)
        {
            return;
        }
        response = response.Remove(0, CommandsNames.ShowChannelListCommand.Length + 1);

        List<string> channelNames = response.Split(";").ToList();
        var columns = channelNames.Select(a => new Text(a));
        var rule = new Rule("[underline blue]Channels[/]");
        AnsiConsole.Write(rule);
        AnsiConsole.Write(new Columns(columns));
    }
}