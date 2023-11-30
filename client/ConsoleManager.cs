using IrcNetCore.Common.Commands;
using Spectre.Console;

namespace IrcNetCoreClient
{
    public class ConsoleManager
    {
        public static void WriteInitializationMessage()
        {
            var rule = new Rule("[underline blue]Initialization[/]");
            AnsiConsole.Write(rule);
        }

        public static void WriteWelcomeMessage()
        {
            var rule = new Rule("[underline blue]Welcome to the IRC Client![/]");
            AnsiConsole.Write(rule);
        }

        public static void WriteInfoMessage(string message)
        {
            AnsiConsole.Markup($"[yellow](INFO): {message}[/]\n");
        }

        public static void WriteErrorMessage(string message)
        {
            AnsiConsole.Markup($"[red](ERROR): {message}[/]\n");
        }

        public static string WriteMenuAndGetSelectedOption()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[underline blue]Menu[/]")
                    .PageSize(10)
                    .AddChoices(new[] { "Join channel", ChannelListCommand.MenuText, "Send message", "Exit" })
            );
        }
    }
}