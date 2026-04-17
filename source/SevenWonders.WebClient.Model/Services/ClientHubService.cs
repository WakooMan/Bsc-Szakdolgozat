using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using WebServer.Contract.Messages.Game.ClientMessages;
using WebServer.Contract.Messages.Game.ServerMessages;
using WebServer.Contract.Messages.Lobby.ClientMessages;
using WebServer.Contract.Messages.Lobby.ServerMessages;

namespace SevenWonders.WebClient.Model.Services
{
    public class ClientHubService: IClientHubService
    {
        public string UserName { get; private set; }

        public ClientHubService(IClientMessageDispatcher clientMessageDispatcher)
        {
            m_clientMessageDispatcher = clientMessageDispatcher;
            m_url = "https://localhost:7206/serverhub";
            UserName = string.Empty;
        }

        public async Task Connect(string userName, string? authToken)
        {
            m_hubConnection = new HubConnectionBuilder()
                            .WithAutomaticReconnect()
                            .AddJsonProtocol(options =>
                            {
                                options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                                options.PayloadSerializerOptions.TypeInfoResolverChain.Insert(
                                    0,
                                    JsonSerializerOptions.Default.TypeInfoResolver!
                                );
                            })
                            .WithUrl(m_url, options =>
                            {
                                options.AccessTokenProvider = () => Task.FromResult(authToken);
                            })
                            .Build();
            m_hubConnection.HandshakeTimeout = TimeSpan.FromSeconds(15);
            m_hubConnection.ServerTimeout = TimeSpan.FromSeconds(30);
            m_hubConnection.On<LobbyServerMessage>(nameof(ReceiveLobbyMessage), ReceiveLobbyMessage);
            m_hubConnection.On<GameServerMessage>(nameof(ReceiveGameMessage), ReceiveGameMessage);
            await m_hubConnection.StartAsync();
            UserName = userName;
        }

        public async Task Disconnect()
        {
            if (m_hubConnection is not null)
            {
                try
                {
                    await m_hubConnection.StopAsync();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    await m_hubConnection.DisposeAsync();
                    m_hubConnection = null;
                }
            }
            UserName = string.Empty;
        }

        public async Task InvokeLobbyCommand(LobbyClientMessage lobbyRequestMessage)
        {
            if (m_hubConnection is not null)
            {
                await m_hubConnection.InvokeAsync<LobbyServerMessage>("LobbyMessageReceived", lobbyRequestMessage);
            }
        }

        public async Task InvokeGameCommand(GameClientMessage gameRequestMessage)
        {
            if (m_hubConnection is not null)
            {
                await m_hubConnection.InvokeAsync<GameServerMessage>("GameMessageReceived", gameRequestMessage);
            }
        }

        private async Task ReceiveLobbyMessage(LobbyServerMessage message)
        {
            if (m_hubConnection is not null)
            {
                await m_clientMessageDispatcher.Dispatch(message);
            }
        }

        private async Task ReceiveGameMessage(GameServerMessage message)
        {
            if (m_hubConnection is not null)
            {
                await m_clientMessageDispatcher.Dispatch(message);
            }
        }

        private HubConnection? m_hubConnection;
        private readonly string m_url;
        private readonly IClientMessageDispatcher m_clientMessageDispatcher;
    }
}
