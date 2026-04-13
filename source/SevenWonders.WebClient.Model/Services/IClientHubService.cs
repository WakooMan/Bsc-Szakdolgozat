using WebServer.Contract.Messages.Game.ClientMessages;
using WebServer.Contract.Messages.Lobby.ClientMessages;

namespace SevenWonders.WebClient.Model.Services
{
    public interface IClientHubService
    {
        Task Connect(string? authToken);

        Task Disconnect();

        Task<bool> InvokeLobbyCommand(LobbyClientMessage lobbyRequestMessage);
        Task<bool> InvokeGameCommand(GameClientMessage gameRequestMessage);
    }
}
