using IrcNetCore.Common;
using IrcNetCoreServer.Entities;

namespace IrcNetCoreServer.Commands;

public class DemoteUserCommand : ICommand
{
    private readonly ChannelManager _channelManager;
    private readonly User? _user;
    string _result = String.Empty;
    public DemoteUserCommand(User? user, ChannelManager channelManager)
    {
        _channelManager = channelManager;
        _user = user;
    }
    public string GetCommandResponse(string parameters)
    {
        return $"{CommandsNames.DemoteUserCommand} {_result ?? String.Empty}";
    }

    public void ProcessCommand(string parameters)
    {
        string[] splittedParameters = parameters.Split(" ");
        if (splittedParameters.Length != 2)
        {
            throw new Exception("Invalid number of parameters!");
        }
        string channelName = splittedParameters[0];
        string username = splittedParameters[1];
        if (_user == null)
        {
            throw new Exception("User is not set!");
        }
        _result = _channelManager.DemoteUser(channelName, _user, username);
    }
}
