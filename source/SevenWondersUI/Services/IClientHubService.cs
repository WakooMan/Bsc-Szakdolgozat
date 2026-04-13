using WebServer.Contract.Messages.Game.ClientMessages;
using WebServer.Contract.Messages.Lobby.ClientMessages;

namespace SevenWondersUI.Services
{
    public interface IClientHubService
    {
        Task Connect(string? authToken);

        Task<bool> InvokeLobbyCommand(LobbyClientMessage lobbyRequestMessage);
        Task<bool> InvokeGameCommand(GameClientMessage gameRequestMessage);
    }
}
