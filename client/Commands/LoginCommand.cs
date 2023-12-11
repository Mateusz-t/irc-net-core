using IrcNetCore.Common;

namespace IrcNetCoreClient.Commands;

public class LoginCommand : ICommand
{
    private string _username;
    public LoginCommand(string username)
    {
        _username = username;
    }
    public string GetCommandRequest()
    {
        return $"{CommandsNames.LoginCommand} {_username}";
    }

    public void ProcessResponse(string response)
    {
        return;
    }
}