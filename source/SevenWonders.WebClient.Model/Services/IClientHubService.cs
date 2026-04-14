using WebServer.Contract.Messages.Game.ClientMessages;
using WebServer.Contract.Messages.Lobby.ClientMessages;

namespace SevenWonders.WebClient.Model.Services
{
    public interface IClientHubService
    {
        Task Connect(string? authToken);

        Task Disconnect();

        Task InvokeLobbyCommand(LobbyClientMessage lobbyRequestMessage);
        Task InvokeGameCommand(GameClientMessage gameRequestMessage);
    }
}
