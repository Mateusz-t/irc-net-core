namespace IrcNetCore.Server
{
    public class ChannelManager
    {
        private readonly Dictionary<string, Channel> _channels = new();

        public void JoinOrCreateChannel(string channelName, User user)
        {
            var foundChannel = _channels[channelName];
            if (foundChannel == null)
            {
                _channels.Add(channelName, new Channel(channelName, user));
            }
            else
            {
                foundChannel.UsersWithRoles.Add(new UserWithRole(user, UserRole.User));
            }
        }

        public void RemoveUserFromChannel(string channelName, User user)
        {
            var foundChannel = GetChannel(channelName);
            var foundUserWithRole = GetUserWithRole(foundChannel, user);

            RemoveUserFromChannel(foundChannel, foundUserWithRole);
        }

        public void PromoteUser(string channelName, User promotingUser, User userToPromote, UserRole roleToPromoteTo)
        {
            var foundChannel = GetChannel(channelName);
            var foundPromotingUserWithRole = GetUserWithRole(foundChannel, promotingUser);
            if (foundPromotingUserWithRole.Role != UserRole.Admin)
            {
                throw new Exception($"User {promotingUser.Username} is not an admin in channel {channelName}");
            }

            var foundUserToPromoteWithRole = GetUserWithRole(foundChannel, userToPromote);
            foundUserToPromoteWithRole.Role = roleToPromoteTo;
        }

        private void RemoveUserFromChannel(Channel channel, UserWithRole userWithRole)
        {
            if (channel.UsersWithRoles.Count == 1)
            {
                _channels.Remove(channel.Name);
            }
            else
            {
                if (userWithRole.Role == UserRole.Admin)
                {
                    var newAdmin = channel.UsersWithRoles.SingleOrDefault(a => a.Role == UserRole.Moderator)
                                ?? channel.UsersWithRoles.SingleOrDefault(a => a.Role == UserRole.User);
                    if (newAdmin != null)
                    {
                        newAdmin.Role = UserRole.Admin;
                    }
                }
                channel.UsersWithRoles.Remove(userWithRole);
            }
        }

        private Channel GetChannel(string channelName)
        {
            var foundChannel = _channels[channelName]
                ?? throw new Exception($"Channel {channelName} does not exist");
            return foundChannel;
        }

        private static UserWithRole GetUserWithRole(Channel channel, User user)
        {
            var foundUserWithRole = channel.UsersWithRoles.SingleOrDefault(a => a.User.Id == user.Id)
                ?? throw new Exception($"User {user.Username} is not in channel {channel.Name}");
            return foundUserWithRole;
        }
    }
}