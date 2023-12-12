using IrcNetCore.Common;
using IrcNetCoreServer.Entities;

namespace IrcNetCoreServer.Commands;

public class PromoteUserCommand : ICommand
{
    private readonly ChannelManager _channelManager;
    private readonly User? _user;
    string _result = String.Empty;
    public PromoteUserCommand(User? user, ChannelManager channelManager)
    {
        _channelManager = channelManager;
        _user = user;
    }
    public string GetCommandResponse(string parameters)
    {
        return $"{CommandsNames.PromoteUserCommand} {_result ?? String.Empty}";
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
        //write those in console
        Console.WriteLine($"User promoted user {username} on channel {channelName}");


        if (_user == null)
        {
            throw new Exception("User is not set!");
        }
        try
        {
            _result = _channelManager.PromoteUser(channelName, _user, username);
        }
        catch (Exception)
        {
            _result = String.Empty;
        }
    }
}
