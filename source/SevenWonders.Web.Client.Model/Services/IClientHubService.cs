using SevenWonders.Web.Server.Contract.Messages.Game.ClientMessages;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ClientMessages;

namespace SevenWonders.Web.Client.Model.Services
{
    public interface IClientHubService
    {
        string UserName { get; }
        Task Connect(string userName, string? authToken);

        Task Disconnect();

        Task InvokeLobbyCommand(LobbyClientMessage lobbyRequestMessage);
        Task InvokeGameCommand(GameClientMessage gameRequestMessage);
    }
}
