using IrcNetCore.Common;

namespace IrcNetCoreClient.Commands;

public class SendMessageCommand : ICommand
{
    private string _channelName;
    private string _message;
    public SendMessageCommand(string channelName, string message)
    {
        _channelName = channelName;
        _message = message;
    }
    public string GetCommandRequest()
    {
        return $"{CommandsNames.SendMessageCommand} {_channelName} {_message}";
    }

    public void ProcessResponse(string response)
    {
        return;
    }
}