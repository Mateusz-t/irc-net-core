using Spectre.Console;

namespace IrcNetCore.Common.Commands;

public class ChannelListCommand : ICommand
{
    public const string COMMAND = "/list";
    public const string MenuText = "Show channels";

    private List<string> _channelNames = new();

    public ChannelListCommand(List<string>? channelNames = null)
    {
        if (channelNames != null)
        {
            _channelNames = channelNames;
        }
    }

    public string GetCommandToSend(string message)
    {
        return COMMAND;
    }

    public string GetCommandResponse(string message)
    {

        return string.Join(",", _channelNames);
    }

    public void ProcessCommand(string message)
    {
        List<string> channelNames = message.Split(',').ToList();
        var columns = channelNames.Select(a => new Text(a));
        var rule = new Rule("[underline blue]Channels[/]");
        AnsiConsole.Write(rule);
        AnsiConsole.Write(new Columns(columns));
    }
}