using Spectre.Console;

namespace IrcNetCoreClient;

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

    public static string AskForUsername()
    {
        return AnsiConsole.Ask<string>("Enter username:");
    }

    public static string WriteMenuAndGetSelectedOption(IEnumerable<string> options)
    {
        options = options.Append("Exit");
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[underline blue]Menu[/]")
                .PageSize(10)
                .AddChoices(options)
        );
    }

    public static string AskForChannelName()
    {
        return AnsiConsole.Ask<string>("Enter channel name:");
    }

    public static void WriteChannelMessage(string channelName)
    {
        var rule = new Rule($"[underline blue]{channelName}[/]");
        AnsiConsole.Write(rule);
        AnsiConsole.Markup($"[green]To see commands write /help[/]\n");
    }

    public static void ShowChannelHelp()
    {
        AnsiConsole.Markup($"[fuchsia]- To see commands write /help[/]\n");
        AnsiConsole.Markup($"[fuchsia]- To see channel users list /users[/]\n");
        AnsiConsole.Markup($"[fuchsia]- To close channel write /close[/]\n");
        AnsiConsole.Markup($"[fuchsia]- To exit channel write /exit[/]\n");
    }

    public static string AskForMessage()
    {
        string? message = null;
        while (message == null)
        {
            message = Console.ReadLine();
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
        return message;
    }
}
