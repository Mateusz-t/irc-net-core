namespace IrcNetCoreServer
{
    class Program
    {
        

        static void Main(string[] args)
        {
            ServerManager serverManager = new();
            serverManager.Run();
        }
    }
}