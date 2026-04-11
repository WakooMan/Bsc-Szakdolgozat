using WebServer.Contract.DataTransferObjects;
using WebServer.Model.Lobby;
using WebServer.Model.PlayerStates;
using WebServer.Model.PlayerStates.Factories;

namespace WebServer.Model.Client
{
    public class PlayerClient : IPlayerClient
    {
        public ApplicationUser ApplicationUser { get; }
        public string ConnectionId { get; }

        public PlayerClient(IPlayerStateFactory playerStateFactory, ApplicationUser applicationUser, string connectionId)
        {
            ConnectionId = connectionId;
            ApplicationUser = applicationUser;
            m_state = playerStateFactory.CreateInMainMenuState(this);
        }

        public void ChangeState(PlayerState playerState)
        {
            m_state = playerState;
        }

        public ILobby CreateLobby(string code, string name)
        {
            return m_state.CreateLobby(code, name);
        }
        public void StartMatchmaking()
        {
            m_state.StartMatchmaking();
        }
        public void ExitMatchmaking()
        {
            m_state.ExitMatchmaking();
        }
        public ILobby JoinLobby(string code)
        {
            return m_state.JoinLobby(code);
        }
        public void LeaveLobby()
        {
            m_state.LeaveLobby();
        }
        public void StartGame()
        {
            m_state.StartGame();
        }
        public void ExitGame()
        {
            m_state.ExitGame();
        }

        public LobbyPlayerDto ToDto(bool isHost)
        {
            return new LobbyPlayerDto(ApplicationUser.UserName ?? string.Empty, isHost);
        }

        private PlayerState m_state;

    }
}
