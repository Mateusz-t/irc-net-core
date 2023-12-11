namespace IrcNetCoreClient;

class Program
{
    static void Main(string[] args)
    {
        ClientManager clientManager = new();
        clientManager.Run();
    }
}
