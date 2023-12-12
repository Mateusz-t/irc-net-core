using System.Net;
using System.Net.Sockets;
using System.Text;

namespace IrcNetCoreClient;

public class SocketManager
{
    private readonly string _IpAddress;
    private readonly int _Port;
    private Socket? _clientSocket;
    public SocketManager(string ipAddress, int port)
    {
        _IpAddress = ipAddress;
        _Port = port;
    }

    public void Connect()
    {
        _clientSocket = CreateSocket();
        _clientSocket.Connect(_IpAddress, _Port);
    }

    public void Close()
    {
        _clientSocket?.Close();
    }
    public void SendMessage(string message)
    {
        if (_clientSocket == null)
            throw new Exception("Socket is not initialized!");

        byte[] messageBytes = Encoding.ASCII.GetBytes(message);
        _clientSocket.Send(messageBytes);
    }

    public string ReceiveMessage()
    {
        if (_clientSocket == null)
            throw new Exception("Socket is not initialized!");
        byte[] buffer = new byte[1024];
        int receivedBytes = _clientSocket.Receive(buffer);
        string message = Encoding.ASCII.GetString(buffer, 0, receivedBytes).Trim();
        return message;
    }

    private Socket CreateSocket()
    {
        ConsoleManager.WriteInfoMessage("Creating socket...");
        IPAddress ipAddress = IPAddress.Parse(_IpAddress);
        IPEndPoint iPEndPoint = new(ipAddress, _Port);
        Socket clientSocket = new(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        ConsoleManager.WriteInfoMessage("Socket created.");
        return clientSocket;
    }
}


