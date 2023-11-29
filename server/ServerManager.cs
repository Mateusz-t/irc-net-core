using System.Net.Sockets;
using System.Text;
using IrcNetCoreServer.Events;
using IrcNetCore.Common;
namespace IrcNetCoreServer
{
    public class ServerManager
    {
        private const int PORT = 12345;
        private const string IP_ADDRESS = "127.0.0.1";       
        private SocketListener? _socketListener;
        private ChannelManager _channelManager = new();

        public void Run()
        {
            _socketListener = new(IP_ADDRESS, PORT);
            _socketListener.Start();
            _socketListener.OnCommandReceived += ProcessCommand;
        }

        public void ProcessCommand(object? sender, CommandReceivedEventArgs e)
        {
            string response = GetCommandResponse(e.Command);
            byte[] encodedCommand = Encoding.ASCII.GetBytes(response);
            e.ClientSocket.Send(encodedCommand);
        }

        public string GetCommandResponse(string command)
        {
            if(_socketListener == null)
                throw new Exception("Socket listener is not initialized!");
            return command switch
            {
                Commands.SHOW_CHANNELS_LIST_COMMAND => _channelManager.GetChannelsList().Aggregate((a, b) => $"{a}, {b}"),
                _ => throw new Exception($"Unknown command: {command}"),
            };
        }
    }
}