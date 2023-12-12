using IrcNetCore.Common;
using IrcNetCoreServer.Events;

namespace IrcNetCoreServer.Commands;

public class CommandFactory
{

    private readonly ChannelManager _channelManager;
    private readonly CommandReceivedEventArgs _commandReceivedEventArgs;

    public CommandFactory(ChannelManager channelManager, CommandReceivedEventArgs commandReceivedEventArgs)
    {
        _channelManager = channelManager;
        _commandReceivedEventArgs = commandReceivedEventArgs;
    }


    public ICommand GetCommand(string command)
    {
        return command switch
        {
            CommandsNames.AckCommand => new AckCommand(),
            CommandsNames.LoginCommand => new LoginCommand(_commandReceivedEventArgs),
            CommandsNames.ShowChannelListCommand => new ShowChannelListCommand(_channelManager),
            CommandsNames.JoinChannelCommand => new JoinChannelCommand(_commandReceivedEventArgs.User, _channelManager),
            CommandsNames.ExitChannelCommand => new ExitChannelCommand(_commandReceivedEventArgs.User, _channelManager),
            CommandsNames.SendMessageCommand => new SendMessageCommand(_commandReceivedEventArgs.User, _channelManager),
            CommandsNames.ShowChannelUsersCommand => new ShowChannelUsersCommand(_channelManager),
            CommandsNames.ShowChannelMessagesCommand => new ShowChannelMessagesCommand(_channelManager),
            CommandsNames.PromoteUserCommand => new PromoteUserCommand(_commandReceivedEventArgs.User, _channelManager),
            CommandsNames.DemoteUserCommand => new DemoteUserCommand(_commandReceivedEventArgs.User, _channelManager),
            _ => throw new Exception("Unknown command!")
        };
    }
}
