using System.Net;
using System.Net.Sockets;
using IrcNetCore.Common;
using IrcNetCoreServer.Commands;
using IrcNetCoreServer.Entities;
using IrcNetCoreServer.Events;

namespace IrcNetCoreServer;

public class SocketListener
{
    // for logging purposes
    private readonly string _name;
    // socket ip address
    private readonly string _ipAddress;
    // socket port
    private readonly int _port;
    // event that is invoked when a command is received
    public event EventHandler<CommandReceivedEventArgs>? OnCommandReceived;
    // irc channel manager
    private readonly ChannelManager _channelManager;
    public SocketListener(string name, string ipAddress, int port, ChannelManager channelManager)
    {
        _name = name;
        _ipAddress = ipAddress;
        _port = port;
        _channelManager = channelManager;
    }

    /// <summary>
    /// Starts listening for clients to connect
    /// </summary>
    /// <param name="sendAck">For command socket, so client knows when it can start sending commands</param>
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
            // accept client connection
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
        if (sendAck)
        {
            // Send ACK to client, so it knows that it can send messages
            clientSocket.EncodeAndSend(new AckCommand().GetCommandResponse(string.Empty));
        }
        while (true)
        {
            // wait for client to send a message
            string message = clientSocket.ReceiveAndDecode();

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
