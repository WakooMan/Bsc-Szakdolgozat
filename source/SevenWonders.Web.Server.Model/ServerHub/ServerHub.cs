using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SevenWonders.Web.Server.Contract.Messages.Game.ClientMessages;
using SevenWonders.Web.Server.Contract.Messages.Game.ServerMessages;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ClientMessages;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages;
using SevenWonders.Web.Server.Model.Client;
using SevenWonders.Web.Server.Model.Client.Factories;
using SevenWonders.Web.Server.Model.MessageHandling;
using SevenWonders.Web.Server.Model.PlayerStates;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SevenWonders.Web.Server.Model.ServerHub
{
    [Authorize]
    public class ServerHub: Hub
    {
        public ServerHub(UserManager<ApplicationUser> userManager, IClientManager clientManager, IPlayerClientFactory playerClientFactory, IServerMessageDispatcher serverMessageDispatcher)
        {
            m_userManager = userManager;
            m_clientManager = clientManager;
            m_playerClientFactory = playerClientFactory;
            m_serverMessageDispatcher = serverMessageDispatcher;
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
                        await Groups.AddToGroupAsync(Context.ConnectionId, nameof(InMainMenu));
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
                await playerClient.LeaveLobby();
            }
            catch (Exception ex)
            {

            }
            try
            {
                await playerClient.ExitGame();
            }
            catch (Exception ex)
            {

            }
            m_clientManager.RemoveClient(playerClient);
            await  base.OnDisconnectedAsync(exception);
        }

        public async Task LobbyMessageReceived(LobbyClientMessage lobbyMessage)
        {
            string connectionId = Context.ConnectionId;
            await m_serverMessageDispatcher.Dispatch(connectionId, lobbyMessage);
        }

        public async Task GameMessageReceived(GameClientMessage gameMessage)
        {
            string connectionId = Context.ConnectionId;
            await m_serverMessageDispatcher.Dispatch(connectionId, gameMessage);
        }

        private readonly IServerMessageDispatcher m_serverMessageDispatcher;
        private readonly IClientManager m_clientManager;
        private readonly IPlayerClientFactory m_playerClientFactory;
        private readonly UserManager<ApplicationUser> m_userManager;
    }
}
