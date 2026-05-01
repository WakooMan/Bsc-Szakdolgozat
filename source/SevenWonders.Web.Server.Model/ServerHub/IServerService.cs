using SevenWonders.Web.Server.Contract.Messages.Game.ServerMessages;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages;

namespace SevenWonders.Web.Server.Model.ServerHub
{
    public interface IServerService
    {
        Task JoinGroup(string connectionId, string groupName);
        Task LeaveGroup(string connectionId, string groupName);
        Task SendGameServerMessageToClient(string connectionId, GameServerMessage message);

        Task SendLobbyServerMessageToClient(string connectionId, LobbyServerMessage message);

        Task SendGameServerMessageToAllClient(string connectionId, GameServerMessage message);

        Task SendLobbyServerMessageToAllClient(string connectionId, LobbyServerMessage message);


        Task SendGameServerMessageToGroup(string groupName, GameServerMessage message, params string[] exceptions);

        Task SendLobbyServerMessageToGroup(string groupName, LobbyServerMessage message, params string[] exceptions);


        Task SendGameServerMessageToGroups(string[] groupNames, GameServerMessage message);

        Task SendLobbyServerMessageToGroups(string[] groupNames, LobbyServerMessage message);
    }
}
