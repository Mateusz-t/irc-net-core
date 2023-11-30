using System.Net.Sockets;

namespace IrcNetCoreServer.Events;

public class CommandReceivedEventArgs : EventArgs
{
    public Socket ClientSocket { get; }
    public string Command { get; }

    public CommandReceivedEventArgs(Socket clientSocket, string command)
    {
        ClientSocket = clientSocket;
        Command = command;
    }
}
