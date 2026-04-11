using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebServer.Contract.Messages.Game.Requests;
using WebServer.Contract.Messages.Game.Responses;
using WebServer.Contract.Messages.Lobby;
using WebServer.Model.MessageHandling;

namespace SevenWonders.WebServer
{
    [Authorize]
    public class ServerHub: Hub
    {
        public ServerHub(IServerMessageDispatcher serverMessageDispatcher, ILobbyMessageHandlers lobbyMessageHandlers)
        {
            m_serverMessageDispatcher = serverMessageDispatcher;
            m_lobbyMessageHandlers = lobbyMessageHandlers;
            m_serverMessageDispatcher.RegisterHandler(lobbyMessageHandlers);
        }

        public async Task<LobbyResponseMessage> LobbyMessageReceived(LobbyRequestMessage lobbyMessage)
        {
            string connectionId = Context.ConnectionId;
            return await m_serverMessageDispatcher.Dispatch(this, connectionId, lobbyMessage);
        }

        public async Task<GameResponseMessage> GameMessageReceived(GameRequestMessage gameMessage)
        {
            string connectionId = Context.ConnectionId;
            return await m_serverMessageDispatcher.Dispatch(this, connectionId, gameMessage);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            m_serverMessageDispatcher.UnregisterHandler(m_lobbyMessageHandlers);
        }

        private readonly IServerMessageDispatcher m_serverMessageDispatcher;
        private readonly ILobbyMessageHandlers m_lobbyMessageHandlers;
    }
}
