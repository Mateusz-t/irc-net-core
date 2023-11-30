using System.Net;
using System.Net.Sockets;
using System.Text;
using IrcNetCore.Common.Commands;
using IrcNetCoreServer.Events;

namespace IrcNetCoreServer;

public class SocketListener
{
    private readonly string _IpAddress;
    private readonly int _Port;
    public event EventHandler<CommandReceivedEventArgs>? OnCommandReceived;
    public SocketListener(string ipAddress, int port)
    {
        _IpAddress = ipAddress;
        _Port = port;
    }

    public void Start()
    {
        Socket listenerSocket = CreateSocket();
        Thread thread = new(() => ListenForClients(listenerSocket));
        thread.Start();
    }

    private void ListenForClients(Socket listenerSocket)
    {
        Console.WriteLine("Waiting for clients...");
        while (true)
        {
            var clientSocket = listenerSocket.Accept();
            Console.WriteLine($"Client {clientSocket.RemoteEndPoint} connected.");
            // clientSocket.Send(Encoding.ASCII.GetBytes("Hello from server!"));
            //create a new thread to receive messages from the client
            Thread thread = new(() => ReceiveMessage(clientSocket));
            thread.Start();
        }
    }

    private void ReceiveMessage(Socket clientSocket)
    {
        // Send ACK to client, so it knows that it can send messages
        AckCommand ackCommand = new();
        clientSocket.Send(Encoding.ASCII.GetBytes(ackCommand.GetCommandToSend(string.Empty)));
        while (true)
        {
            byte[] buffer = new byte[1024];
            int receivedBytes = clientSocket.Receive(buffer);
            string message = Encoding.ASCII.GetString(buffer, 0, receivedBytes).Trim();

            if (!IsConnected(clientSocket))
            {
                Console.WriteLine($"Client {clientSocket.RemoteEndPoint} disconnected.");
                break;
            }
            Console.WriteLine($"Received {clientSocket.RemoteEndPoint}: {message}");
            OnCommandReceived?.Invoke(this, new CommandReceivedEventArgs(clientSocket, message));
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
        Console.WriteLine("Creating socket...");
        Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress iPAddress = IPAddress.Parse(_IpAddress);
        IPEndPoint iPEndPoint = new(iPAddress, _Port);
        socket.Bind(iPEndPoint);
        socket.Listen(5);
        Console.WriteLine("Socket created.");
        return socket;
    }
}
