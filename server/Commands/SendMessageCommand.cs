using IrcNetCoreServer.Entities;

namespace IrcNetCoreServer.Commands;

public class SendMessageCommand : ICommand
{
    private readonly ChannelManager _channelManager;
    private readonly User? _user;
    public SendMessageCommand(User? user, ChannelManager channelManager)
    {
        _channelManager = channelManager;
        _user = user;
    }
    public string GetCommandResponse(string parameters)
    {
        List<string> splittedParameters = parameters.Split(" ").ToList();
        if (_user == null)
        {
            throw new Exception("User is not set!");
        }
        if (splittedParameters.Count < 2)
        {
            throw new Exception("Invalid parameters!");
        }
        string message = string.Join(" ", splittedParameters.Skip(1));
        return $"({_user.Username}, {DateTime.Now:HH:mm:ss}): {message}";
    }

    public void ProcessCommand(string parameters)
    {
        if (_user == null)
        {
            throw new Exception("User is not set!");
        }
        Console.WriteLine(parameters);
        List<string> splittedParameters = parameters.Split(" ").ToList();
        if (splittedParameters.Count < 2)
        {
            throw new Exception("Invalid parameters!");
        }
        string channelName = splittedParameters[0];
        //join all parameters except first one
        string message = string.Join(" ", splittedParameters.Skip(1));
        _channelManager.SendMessageToChannel(channelName, _user, message);
    }
}