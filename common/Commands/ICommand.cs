namespace IrcNetCore.Common.Commands;

public interface ICommand
{
    string GetCommandToSend(string message);
    string GetCommandResponse(string message);
    void ProcessCommand(string message);
}
