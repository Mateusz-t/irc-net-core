using IrcNetCoreServer.Entities;

namespace IrcNetCoreServer.Commands;

public class ExitChannelCommand : ICommand
{
    private readonly ChannelManager _channelManager;
    private readonly User? _user;
    public ExitChannelCommand(User? user, ChannelManager channelManager)
    {
        _channelManager = channelManager;
        _user = user;
    }
    public string GetCommandResponse(string channelName)
    {
        return channelName;
    }

    public void ProcessCommand(string channelName)
    {
        if (_user == null)
        {
            throw new Exception("User is not set!");
        }
        _channelManager.RemoveUserFromChannel(channelName, _user);
    }
}
