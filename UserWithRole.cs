namespace IrcNetCore.Server
{
    public class UserWithRole
    {
        public UserWithRole(User user, UserRole role)
        {
            User = user;
            Role = role;
        }

        public User User { get; set; }
        public UserRole Role { get; set; }
    }
}