using IrcNetCore.Common;

namespace IrcNetCoreClient.Commands;

public class LoginCommand : ICommand
{
    public string GetCommandRequest(string username)
    {
        return $"{CommandsNames.LoginCommand} {username}";
    }

    public void ProcessResponse(string response)
    {
        return;
    }
}