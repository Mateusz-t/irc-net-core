using IrcNetCore.Common;

namespace IrcNetCoreServer.Commands;
public class ShowChannelMessagesCommand : ICommand
{
    private readonly ChannelManager _channelManager;

    public ShowChannelMessagesCommand(ChannelManager channelManager)
    {
        _channelManager = channelManager;
    }
    public string GetCommandResponse(string channelName)
    {
        return $"{CommandsNames.ShowChannelMessagesCommand} {_channelManager.GetChannelMessages(channelName)}";

    }

    public void ProcessCommand(string parameters)
    {
        return;
    }
}
