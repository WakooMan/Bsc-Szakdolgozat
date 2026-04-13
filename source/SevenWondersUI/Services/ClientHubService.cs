using Microsoft.AspNetCore.SignalR.Client;
using SevenWonders.WebClient.Model;
using WebServer.Contract.Messages.Game.ClientMessages;
using WebServer.Contract.Messages.Game.ServerMessages;
using WebServer.Contract.Messages.Lobby.ClientMessages;
using WebServer.Contract.Messages.Lobby.ServerMessages;

namespace SevenWondersUI.Services
{
    public class ClientHubService: IClientHubService
    {
        public ClientHubService(IClientMessageDispatcher clientMessageDispatcher)
        {
            m_clientMessageDispatcher = clientMessageDispatcher;
            m_url = "https://localhost:7206/serverhub";
            m_connectionBuilder = new HubConnectionBuilder()
            .WithAutomaticReconnect();
        }

        public async Task Connect(string? authToken)
        {
            m_hubConnection = m_connectionBuilder
                            .WithUrl(m_url, options =>
                            {
                                options.AccessTokenProvider = () => Task.FromResult(authToken);
                            })
                            .Build();
            m_hubConnection.HandshakeTimeout = TimeSpan.FromSeconds(15);
            m_hubConnection.ServerTimeout = TimeSpan.FromSeconds(30);
            await m_hubConnection.StartAsync();
        }

        public async Task<bool> InvokeLobbyCommand(LobbyClientMessage lobbyRequestMessage)
        {
            if (m_hubConnection is not null)
            {
                LobbyServerMessage message = await m_hubConnection.InvokeAsync<LobbyServerMessage>("LobbyMessageReceived", lobbyRequestMessage);
                return await m_clientMessageDispatcher.Dispatch(message);
            }

            return false;
        }

        public async Task<bool> InvokeGameCommand(GameClientMessage gameRequestMessage)
        {
            if (m_hubConnection is not null)
            {
                GameServerMessage message = await m_hubConnection.InvokeAsync<GameServerMessage>("GameMessageReceived", gameRequestMessage);
                return await m_clientMessageDispatcher.Dispatch(message);
            }

            return false;
        }

        private HubConnection? m_hubConnection;
        private readonly IHubConnectionBuilder m_connectionBuilder;
        private readonly string m_url;
        private readonly IClientMessageDispatcher m_clientMessageDispatcher;
    }
}
