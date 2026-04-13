using WebServer.Contract.DataTransferObjects;
using WebServer.Contract.Messages.Lobby.ServerMessages;
using WebServer.Model.PlayerStates;

namespace WebServer.Model.Client
{
    public interface IPlayerClient
    {
        ApplicationUser ApplicationUser { get; }
        string ConnectionId { get; }

        void ChangeState(PlayerState playerState);

        Task<LobbyServerMessage> CreateLobby(string name);

        Task<LobbyServerMessage> StartMatchmaking();

        Task<LobbyServerMessage> ExitMatchmaking();

        Task<LobbyServerMessage> JoinLobby(string code);
        
        Task<LobbyServerMessage> LeaveLobby();

        Task<LobbyServerMessage> StartGame();

        Task<LobbyServerMessage> ExitGame();
        Task<LobbyServerMessage> WriteChatMessage(string message);

        LobbyPlayerDto ToDto(bool isHost);
        
    }
}
