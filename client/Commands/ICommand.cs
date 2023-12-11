namespace IrcNetCoreClient.Commands;

public interface ICommand
{
    string GetCommandRequest();
    void ProcessResponse(string response);
}
