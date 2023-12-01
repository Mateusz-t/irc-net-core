using IrcNetCore.Common;

namespace IrcNetCoreClient.Commands;

public class CommandFactory
{

    public ICommand GetCommand(string command)
    {
        return command switch
        {
            CommandsNames.AckCommand => new AckCommand(),
            CommandsNames.LoginCommand => new LoginCommand(),
            CommandsNames.ShowChannelListCommand => new ShowChannelListCommand(),
            CommandsNames.JoinChannelCommand => new JoinChannelCommand(),
            CommandsNames.SendMessageCommand => new SendMessageCommand(),
            CommandsNames.ExitChannelCommand => new ExitChannelCommand(),
            CommandsNames.ShowChannelUsersCommand => new ShowChannelUsersCommand(),
            _ => throw new Exception("Unknown command!")
        };
    }
}