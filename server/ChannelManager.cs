using System.Net.Sockets;
using System.Text;
using IrcNetCore.Common;
using IrcNetCoreServer.Entities;

namespace IrcNetCoreServer;

public class ChannelManager
{
    private readonly Dictionary<string, Channel> _channels = new();

    public string JoinOrCreateChannel(string channelName, User user)
    {
        var foundChannel = _channels.SingleOrDefault(a => a.Key == channelName).Value;
        if (foundChannel == null)
        {
            _channels.Add(channelName, new Channel(channelName, user));
            Console.WriteLine($"User {user.Username} created channel {channelName}");
            return channelName;
        }
        else if (!foundChannel.UsersWithRoles.Any(a => a.User.Username == user.Username))
        {
            foundChannel.UsersWithRoles.Add(new UserWithRole(user, UserRole.User));
            Console.WriteLine($"User {user.Username} joined channel {channelName}");
            return channelName;
        }
        Console.WriteLine($"User {user.Username} could not join channel {channelName}");
        return String.Empty;
    }

    public void StartListeningToChannel(string channelName, Socket socket)
    {
        var foundChannel = GetChannel(channelName);
        foundChannel.ListeningSockets.Add(socket);
    }

    public void StopListeningOnSocket(Socket socket)
    {
        foreach (var channel in _channels.Values)
        {
            channel.ListeningSockets.Remove(socket);
        }
    }

    public string GetChannelMessages(string channelName)
    {
        var foundChannel = GetChannel(channelName);
        var messages = foundChannel.Messages;
        var messagesString = string.Join(";", messages.Select(a => $"{a.FormattedMessage}"));
        return messagesString;
    }

    public string GetChannelUsers(string channelName)
    {
        var foundChannel = GetChannel(channelName);
        var users = foundChannel.UsersWithRoles.OrderBy(a => a.Role);
        var usersString = string.Join(";", users.Select(a => $"{a.User.Username}: {a.Role}"));
        return usersString;
    }

    public void SendMessageToChannel(string channelName, User user, string message)
    {
        var foundChannel = GetChannel(channelName);
        var foundUserWithRole = GetUserWithRole(foundChannel, user.Username);
        Message createdMessage = new(message, foundUserWithRole.User);
        foundChannel.SendMessage(createdMessage);
        SendMessageToUsersConnectedToChannel(foundChannel, createdMessage);
    }

    public static void SendMessageToUsersConnectedToChannel(Channel foundChannel, Message createdMessage)
    {
        foreach (var socket in foundChannel.ListeningSockets)
        {
            string message = $"{CommandsNames.NotifyChannelUsersCommand} {createdMessage.FormattedMessage}";
            byte[] encodedMessage = Encoding.ASCII.GetBytes(message);
            socket.Send(encodedMessage);
            Console.WriteLine($"(MessageSocket) Send {socket.RemoteEndPoint}: {message}");
        }
    }

    public void RemoveUserFromChannel(string channelName, User user)
    {
        var foundChannel = GetChannel(channelName);
        var foundUserWithRole = GetUserWithRole(foundChannel, user.Username);

        RemoveUserFromChannel(foundChannel, foundUserWithRole);
    }

    public string PromoteUser(string channelName, User promotingUser, string usernameToPromote)
    {
        if (promotingUser.Username == usernameToPromote)
        {
            throw new Exception($"User {promotingUser.Username} cannot promote himself");
        }
        var foundChannel = GetChannel(channelName);
        var foundPromotingUserWithRole = GetUserWithRole(foundChannel, promotingUser.Username);
        if (foundPromotingUserWithRole.Role != UserRole.Admin)
        {
            throw new Exception($"User {promotingUser.Username} is not an admin in channel {channelName}");
        }

        var foundUserToPromoteWithRole = GetUserWithRole(foundChannel, usernameToPromote);
        foundUserToPromoteWithRole.Role += 1;
        // admin is the highest role
        if (foundUserToPromoteWithRole.Role > UserRole.Admin)
        {
            throw new Exception($"Cannot promote user {usernameToPromote} any further");
        }
        if (foundUserToPromoteWithRole.Role == UserRole.Admin)
        {
            // only one admin is allowed
            foundPromotingUserWithRole.Role = UserRole.Moderator;
        }
        Console.WriteLine($"Promoted user {usernameToPromote} to {foundUserToPromoteWithRole.Role} on channel {channelName}");
        return foundUserToPromoteWithRole.Role.ToString();
    }

    public string DemoteUser(string channelName, User demotingUser, string usernameToDemote)
    {
        if (demotingUser.Username == usernameToDemote)
        {
            throw new Exception($"User {demotingUser.Username} cannot demote himself");
        }
        var foundChannel = GetChannel(channelName);
        var foundDemotingUserWithRole = GetUserWithRole(foundChannel, demotingUser.Username);
        if (foundDemotingUserWithRole.Role != UserRole.Admin)
        {
            throw new Exception($"User {demotingUser.Username} is not an admin in channel {channelName}");
        }
        var foundUserToDemoteWithRole = GetUserWithRole(foundChannel, usernameToDemote);
        foundUserToDemoteWithRole.Role -= 1;
        // user is the lowest role
        if (foundUserToDemoteWithRole.Role < UserRole.User)
        {
            throw new Exception($"Cannot demote user {usernameToDemote} any further");
        }
        Console.WriteLine($"Demoted user {usernameToDemote} to {foundUserToDemoteWithRole.Role} on channel {channelName}");

        return foundUserToDemoteWithRole.Role.ToString();
    }

    public string GetChannelsList()
    {
        var channelsString = string.Join(";", _channels.Keys);
        return channelsString;
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
        var foundChannel = _channels.SingleOrDefault(a => a.Key == channelName).Value
            ?? throw new Exception($"Channel {channelName} does not exist");
        return foundChannel;
    }

    private static UserWithRole GetUserWithRole(Channel channel, string username)
    {
        var foundUserWithRole = channel.UsersWithRoles.SingleOrDefault(a => a.User.Username == username)
            ?? throw new Exception($"User {username} is not in channel {channel.Name}");
        return foundUserWithRole;
    }
}
