namespace IrcNetCoreServer.Entities;

public class Message
{
    public Message(string text, User user)
    {
        // remove ';' from text to avoid injection
        Text = text.Replace(';', ',');
        User = user;
    }

    public string Text { get; set; }
    public User User { get; set; }
    public DateTime SendTime { get; set; } = DateTime.Now;
    public string FormattedMessage => $"({User.Username}, {SendTime:HH:mm:ss}): {Text}";
}
