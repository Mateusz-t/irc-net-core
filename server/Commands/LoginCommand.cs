using IrcNetCoreServer.Entities;
using IrcNetCoreServer.Events;

namespace IrcNetCoreServer.Commands;

public class LoginCommand : ICommand
{
    private readonly CommandReceivedEventArgs _commandReceivedEventArgs;

    public LoginCommand(CommandReceivedEventArgs commandReceivedEventArgs)
    {
        _commandReceivedEventArgs = commandReceivedEventArgs;
    }

    public string GetCommandResponse(string username)
    {
        return username;
    }

    public void ProcessCommand(string username)
    {
        _commandReceivedEventArgs.User = new User(username, _commandReceivedEventArgs.ClientSocket);
    }
}
