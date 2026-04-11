using WebServer.Contract.DataTransferObjects;
using WebServer.Model.Lobby;
using WebServer.Model.PlayerStates;

namespace WebServer.Model.Client
{
    public interface IPlayerClient
    {
        ApplicationUser ApplicationUser { get; }
        string ConnectionId { get; }

        void ChangeState(PlayerState playerState);

        ILobby CreateLobby(string code, string name);
        
        void StartMatchmaking();
        
        void ExitMatchmaking();
        
        ILobby JoinLobby(string code);
        
        void LeaveLobby();
        
        void StartGame();
        
        void ExitGame();

        LobbyPlayerDto ToDto(bool isHost);
        
    }
}
