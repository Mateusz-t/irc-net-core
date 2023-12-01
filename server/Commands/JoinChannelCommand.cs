using IrcNetCore.Common;
using IrcNetCoreServer.Entities;

namespace IrcNetCoreServer.Commands;

public class JoinChannelCommand : ICommand
{
    private readonly ChannelManager _channelManager;
    private readonly User? _user;
    public JoinChannelCommand(User? user, ChannelManager channelManager)
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
        Console.WriteLine($"User {_user.Username} joined channel {channelName}");
        _channelManager.JoinOrCreateChannel(channelName, _user);
    }
}
