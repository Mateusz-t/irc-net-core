namespace IrcNetCoreServer
{
    public class Message
    {
        public Message(string text, User user)
        {
            Text = text;
            User = user;
        }

        public string Text { get; set; }
        public User User { get; set; }
        public DateTime SendTime { get; set; } = DateTime.Now;
    }
}