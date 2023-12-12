using System.Net;
using System.Net.Sockets;
using System.Text;
using IrcNetCoreServer.Commands;
using IrcNetCoreServer.Entities;
using IrcNetCoreServer.Events;

namespace IrcNetCoreServer;

public class SocketListener
{
    private readonly string _name;
    private readonly string _ipAddress;
    private readonly int _port;
    public event EventHandler<CommandReceivedEventArgs>? OnCommandReceived;
    private readonly ChannelManager _channelManager;
    public SocketListener(string name, string ipAddress, int port, ChannelManager channelManager)
    {
        _name = name;
        _ipAddress = ipAddress;
        _port = port;
        _channelManager = channelManager;
    }

    public void Start(bool sendAck = false)
    {
        Socket listenerSocket = CreateSocket();
        Thread thread = new(() => ListenForClients(listenerSocket, sendAck));
        thread.Start();
    }

    private void ListenForClients(Socket listenerSocket, bool sendAck = false)
    {
        Console.WriteLine($"({_name}) Waiting for clients...");
        while (true)
        {
            var clientSocket = listenerSocket.Accept();
            Console.WriteLine($"({_name}) Client {clientSocket.RemoteEndPoint} connected.");
            //create a new thread to receive messages from the client
            Thread thread = new(() => ReceiveMessage(clientSocket, sendAck));
            thread.Start();
        }
    }

    private void ReceiveMessage(Socket clientSocket, bool sendAck = false)
    {
        User? user = null;
        // Send ACK to client, so it knows that it can send messages
        if (sendAck)
        {
            AckCommand ackCommand = new();
            clientSocket.Send(Encoding.ASCII.GetBytes(ackCommand.GetCommandResponse(string.Empty)));
        }
        while (true)
        {
            byte[] buffer = new byte[1024];
            int receivedBytes = clientSocket.Receive(buffer);
            string message = Encoding.ASCII.GetString(buffer, 0, receivedBytes).Trim();

            if (!IsConnected(clientSocket))
            {
                Console.WriteLine($"({_name}) Client {clientSocket.RemoteEndPoint} disconnected.");
                _channelManager.StopListeningOnSocket(clientSocket);
                break;
            }
            Console.WriteLine($"({_name}) Received {clientSocket.RemoteEndPoint}: {message}");
            var eventArgs = new CommandReceivedEventArgs(clientSocket, message, user);
            OnCommandReceived?.Invoke(this, eventArgs);
            // if user is not set, it means that it is a new user
            // first message from a client is always a nickname
            user = eventArgs.User;
        }
    }

    private bool IsConnected(Socket clientSocket)
    {
        try
        {
            return !(clientSocket.Poll(1, SelectMode.SelectRead) && clientSocket.Available == 0);
        }
        catch (SocketException)
        {
            return false;
        }
    }

    private Socket CreateSocket()
    {
        Console.WriteLine($"({_name}) Creating socket...");
        Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress iPAddress = IPAddress.Parse(_ipAddress);
        IPEndPoint iPEndPoint = new(iPAddress, _port);
        socket.Bind(iPEndPoint);
        socket.Listen(5);
        Console.WriteLine($"({_name}) Socket created.");
        return socket;
    }
}
