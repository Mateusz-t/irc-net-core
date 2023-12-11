using IrcNetCore.Common;
using IrcNetCoreServer.Entities;

namespace IrcNetCoreServer.Commands;

public class JoinChannelCommand : ICommand
{
    private readonly ChannelManager _channelManager;
    private readonly User? _user;
    string _result = String.Empty;
    public JoinChannelCommand(User? user, ChannelManager channelManager)
    {
        _channelManager = channelManager;
        _user = user;
    }
    public string GetCommandResponse(string parameters)
    {
        return $"{CommandsNames.JoinChannelCommand} {_result ?? String.Empty}";
    }

    public void ProcessCommand(string channelName)
    {
        if (_user == null)
        {
            throw new Exception("User is not set!");
        }
        _result = _channelManager.JoinOrCreateChannel(channelName, _user);
        if (_result != String.Empty)
        {
            Console.WriteLine($"User {_user.Username} joined channel {channelName}");
        }
        else
        {
            Console.WriteLine($"User {_user.Username} could not join channel {channelName}");
        }
    }
}
