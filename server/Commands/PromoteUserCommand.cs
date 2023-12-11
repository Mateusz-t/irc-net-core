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

    public void ProcessCommand(string paramters)
    {
        string[] parameters = paramters.Split(" ");
        if (parameters.Length != 2)
        {
            throw new Exception("Invalid number of parameters!");
        }
        string channelName = parameters[0];
        string username = parameters[1];
        //write those in console
        Console.WriteLine($"User  promoted user {username} to channel {channelName}");


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
