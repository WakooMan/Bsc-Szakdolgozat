using SevenWonders.Web.Server.Contract.DataTransferObjects;
using SevenWonders.Web.Server.Model.PlayerStates;

namespace SevenWonders.Web.Server.Model.Client
{
    public interface IPlayerClient
    {
        ApplicationUser ApplicationUser { get; }
        string ConnectionId { get; }
        PlayerState CurrentState { get; }

        void ChangeState(PlayerState playerState);

        Task CreateLobby(string name);

        Task StartMatchmaking();

        Task ExitMatchmaking();

        Task JoinLobby(string code);
        
        Task LeaveLobby();

        Task StartGame();

        Task ExitGame();
        Task WriteChatMessage(string message);

        LobbyPlayerDto ToDto(bool isHost);
        
    }
}
