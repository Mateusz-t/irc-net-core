namespace IrcNetCoreClient.Commands;

public interface ICommand
{
    string GetCommandRequest(string parameters);
    void ProcessResponse(string response);
}
