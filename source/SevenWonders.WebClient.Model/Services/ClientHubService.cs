using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using SevenWonders.WebClient.Model;
using System.Text.Json;
using WebServer.Contract.Messages.Game.ClientMessages;
using WebServer.Contract.Messages.Game.ServerMessages;
using WebServer.Contract.Messages.Lobby.ClientMessages;
using WebServer.Contract.Messages.Lobby.ServerMessages;

namespace SevenWonders.WebClient.Model.Services
{
    public class ClientHubService: IClientHubService
    {
        public ClientHubService(IClientMessageDispatcher clientMessageDispatcher)
        {
            m_clientMessageDispatcher = clientMessageDispatcher;
            m_url = "https://localhost:7206/serverhub";
        }

        public async Task Connect(string? authToken)
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
