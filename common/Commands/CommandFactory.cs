namespace IrcNetCore.Common.Commands;

public class CommandFactory
{
    private List<string> _channelNames = new();

    public CommandFactory(List<string> channelNames)
    {
        _channelNames = channelNames;
    }

    public ICommand GetCommand(string command)
    {
        return command switch
        {
            ChannelListCommand.COMMAND => new ChannelListCommand(_channelNames),
            AckCommand.COMMAND => new AckCommand(),
            _ => throw new Exception("Unknown command!")
        };
    }
}