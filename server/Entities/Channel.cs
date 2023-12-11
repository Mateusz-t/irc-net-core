using System.Net.Sockets;

namespace IrcNetCoreServer.Entities;

public class Channel
{
    public Channel(string name, User admin)
    {
        Name = name;
        UsersWithRoles = new List<UserWithRole>() { new(admin, UserRole.Admin) };
        Messages = new List<Message>();
        ListeningSockets = new List<Socket>();
    }
    public string Name { get; set; }
    public List<UserWithRole> UsersWithRoles { get; set; }
    public List<Message> Messages { get; set; }
    public event EventHandler<Message>? MessageAdded;
    public List<Socket> ListeningSockets { get; set; }

    public void SendMessage(Message message)
    {
        Messages.Add(message);
        MessageAdded?.Invoke(this, message);
    }
}
