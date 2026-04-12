using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using WebServer.Contract.Messages.Game.Requests;
using WebServer.Contract.Messages.Game.Responses;
using WebServer.Contract.Messages.Lobby;
using WebServer.Model.Client;
using WebServer.Model.Client.Factories;
using WebServer.Model.MessageHandling;

namespace SevenWonders.WebServer
{
    [Authorize]
    public class ServerHub: Hub
    {
        public ServerHub(UserManager<ApplicationUser> userManager, IClientManager clientManager, IPlayerClientFactory playerClientFactory, IServerMessageDispatcher serverMessageDispatcher, ILobbyMessageHandlers lobbyMessageHandlers)
        {
            m_userManager = userManager;
            m_clientManager = clientManager;
            m_playerClientFactory = playerClientFactory;
            m_serverMessageDispatcher = serverMessageDispatcher;
            m_lobbyMessageHandlers = lobbyMessageHandlers;
            m_serverMessageDispatcher.RegisterHandler(lobbyMessageHandlers);
        }

        public override async Task OnConnectedAsync()
        {
            var user = Context.User;
            if (user is not null)
            {
                ApplicationUser? applicationUser = await m_userManager.GetUserAsync(user);
                if (applicationUser is not null)
                {
                    IPlayerClient playerClient =  m_playerClientFactory.Create(applicationUser, Context.ConnectionId);
                    if (m_clientManager.AddClient(playerClient))
                    {
                        await base.OnConnectedAsync();
                        return;
                    }
                }
            }

            Context.Abort();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            IPlayerClient playerClient = m_clientManager.GetClient(Context.ConnectionId);
            try
            {
                playerClient.LeaveLobby();
            }
            catch (Exception ex)
            {

            }
            try
            {
                playerClient.ExitGame();
            }
            catch (Exception ex)
            {

            }
            m_clientManager.RemoveClient(playerClient);
            await  base.OnDisconnectedAsync(exception);
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
        private readonly IClientManager m_clientManager;
        private readonly IPlayerClientFactory m_playerClientFactory;
        private readonly UserManager<ApplicationUser> m_userManager;
    }
}
