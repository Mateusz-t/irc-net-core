using IrcNetCore.Common;
using Spectre.Console;

namespace IrcNetCoreClient.Commands;

public class ShowChannelMessagesCommand : ICommand
{
    private string _channelName;
    public ShowChannelMessagesCommand(string channelName)
    {
        _channelName = channelName;
    }
    public string GetCommandRequest()
    {
        return $"{CommandsNames.ShowChannelMessagesCommand} {_channelName}";
    }

    public void ProcessResponse(string response)
    {
        if (response.Length < CommandsNames.ShowChannelMessagesCommand.Length + 1)
        {
            return;
        }
        response = response.Remove(0, CommandsNames.ShowChannelMessagesCommand.Length + 1);
        List<string> messages = response.Split(";").ToList();
        foreach (var message in messages)
        {
            AnsiConsole.Markup($"{message}\n");
        }
    }
}