using System.Net.Sockets;
using IrcNetCoreServer.Entities;

namespace IrcNetCoreServer.Events;

public class CommandReceivedEventArgs : EventArgs
{
    public Socket ClientSocket { get; }
    public string Command { get; }
    public User? User { get; set; }

    public CommandReceivedEventArgs(Socket clientSocket, string command, User? user = null)
    {
        ClientSocket = clientSocket;
        Command = command;
        User = user;
    }
}
