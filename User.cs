namespace IrcNetCore.Server
{
    public class User
    {
        public User(string username)
        {
            Username = username;
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Username { get; set; }
    }
}