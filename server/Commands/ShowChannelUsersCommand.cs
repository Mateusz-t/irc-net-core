namespace IrcNetCoreServer.Commands;

public class ShowChannelUsersCommand : ICommand
{
    private readonly ChannelManager _channelManager;

    public ShowChannelUsersCommand(ChannelManager channelManager)
    {
        _channelManager = channelManager;
    }
    public string GetCommandResponse(string channelName)
    {
        return _channelManager.GetChannelUsers(channelName);
    }

    public void ProcessCommand(string channelName)
    {
        return;
    }
}
