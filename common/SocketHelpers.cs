using System.Net.Sockets;
using System.Text;

namespace IrcNetCore.Common;

public static class SocketHelpers
{
    public static int EncodeAndSend(this Socket socket, string data)
    {
        return socket.Send(Encoding.ASCII.GetBytes(data));
    }

    public static string ReceiveAndDecode(this Socket socket)
    {
        byte[] buffer = new byte[1024];
        int receivedBytes = socket.Receive(buffer);
        string message = Encoding.ASCII.GetString(buffer, 0, receivedBytes).Trim();
        return message;
    }

}