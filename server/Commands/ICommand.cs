namespace IrcNetCoreServer.Commands;

public interface ICommand
{
    string GetCommandResponse(string parameters);
    void ProcessCommand(string parameters);
}
