namespace IrcNetCore.Server
{
    public class Channel
    {
        public Channel(string name, User admin)
        {
            Name = name;
            UsersWithRoles = new List<UserWithRole>() { new(admin, UserRole.Admin) };
            Messages = new List<Message>();
        }
        public string Name { get; set; }
        public List<UserWithRole> UsersWithRoles { get; set; }
        public List<Message> Messages { get; set; }
        public event EventHandler<Message>? MessageAdded;

        public void SendMessage(Message message)
        {
            Messages.Add(message);
            MessageAdded?.Invoke(this, message);
        }
    }
}