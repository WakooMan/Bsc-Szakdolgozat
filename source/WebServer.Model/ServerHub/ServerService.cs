using Microsoft.AspNetCore.SignalR;
using WebServer.Contract.Messages.Game.ServerMessages;
using WebServer.Contract.Messages.Lobby.ServerMessages;

namespace WebServer.Model.ServerHub
{
    public class ServerService: IServerService
    {

        public ServerService(IHubContext<ServerHub> hubContext)
        {
            m_hubContext = hubContext;
        }

        public async Task JoinGroup(string connectionId, string groupName)
        {
            await m_hubContext.Groups.AddToGroupAsync(connectionId, groupName);
        }



        public async Task LeaveGroup(string connectionId, string groupName)
        {
            await m_hubContext.Groups.RemoveFromGroupAsync(connectionId, groupName);
        }
        public async Task SendGameServerMessageToClient(string connectionId, GameServerMessage message)
        {
            await m_hubContext.Clients.Client(connectionId).SendAsync("ReceiveGameMessage", message);
        }

        public async Task SendLobbyServerMessageToClient(string connectionId, LobbyServerMessage message)
        {
            await m_hubContext.Clients.Client(connectionId).SendAsync("ReceiveLobbyMessage", message);
        }

        public async Task SendGameServerMessageToAllClient(string connectionId, GameServerMessage message)
        {
            await m_hubContext.Clients.All.SendAsync("ReceiveGameMessage", message);
        }

        public async Task SendLobbyServerMessageToAllClient(string connectionId, LobbyServerMessage message)
        {
            await m_hubContext.Clients.All.SendAsync("ReceiveLobbyMessage", message);
        }


        public async Task SendGameServerMessageToGroup(string groupName, GameServerMessage message)
        {
            await m_hubContext.Clients.Group(groupName)
                .SendAsync("ReceiveGameMessage", message);
        }

        public async Task SendLobbyServerMessageToGroup(string groupName, LobbyServerMessage message)
        {
            await m_hubContext.Clients.Group(groupName)
                .SendAsync("ReceiveLobbyMessage", message);
        }


        public async Task SendGameServerMessageToGroups(string[] groupNames, GameServerMessage message)
        {
            await m_hubContext.Clients.Groups(groupNames)
                .SendAsync("ReceiveGameMessage", message);
        }

        public async Task SendLobbyServerMessageToGroups(string[] groupNames, LobbyServerMessage message)
        {
            await m_hubContext.Clients.Groups(groupNames)
                .SendAsync("ReceiveLobbyMessage", message);
        }

        private readonly IHubContext<ServerHub> m_hubContext;
    }
}
