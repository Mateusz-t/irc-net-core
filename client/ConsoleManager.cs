using Spectre.Console;

namespace IrcNetCoreClient
{
    public class ConsoleManager
    {
        public static void WriteWelcomeMessage()
        {
            var rule = new Rule("[underline blue]Welcome to the IRC Client![/]");
            AnsiConsole.Write(rule);
        }

        public static void WriteSocketErrorMessage()
        {
            AnsiConsole.Markup($"[underline red]Could not connect to a server![/]\n");
        }

        public static void ShowChannelsList(List<string> channelsNames)
        {
            var rows = channelsNames.Select(a => new Text(a));
            AnsiConsole.Write(new Rows(rows));
        }
    }
}