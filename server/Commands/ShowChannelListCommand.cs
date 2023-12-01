namespace IrcNetCoreServer.Commands;

public class ShowChannelListCommand : ICommand
{
    private readonly ChannelManager _channelManager;

    public ShowChannelListCommand(ChannelManager channelManager)
    {
        _channelManager = channelManager;
    }
    public string GetCommandResponse(string parameters)
    {
        return string.Join(";", _channelManager.GetChannelsList());
    }

    public void ProcessCommand(string parameters)
    {
        return;
    }
}
