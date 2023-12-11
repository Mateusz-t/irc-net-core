using System.Net.Sockets;

namespace IrcNetCoreServer.Entities;

public class User
{
    public User(string username, Socket socket)
    {
        Username = username;
        Socket = socket;
    }

    public string Username { get; set; }
    public Socket Socket { get; set; }
}
