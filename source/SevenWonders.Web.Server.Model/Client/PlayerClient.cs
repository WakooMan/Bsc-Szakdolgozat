using SevenWonders.Web.Server.Contract.DataTransferObjects;
using SevenWonders.Web.Server.Model.PlayerStates;
using SevenWonders.Web.Server.Model.PlayerStates.Factories;

namespace SevenWonders.Web.Server.Model.Client
{
    public class PlayerClient : IPlayerClient
    {
        public ApplicationUser ApplicationUser { get; }
        public string ConnectionId { get; }

        public PlayerState CurrentState => m_state;

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

        public Task CreateLobby(string name)
        {
            return m_state.CreateLobby(name);
        }
        public Task StartMatchmaking()
        {
            return m_state.StartMatchmaking();
        }
        public Task ExitMatchmaking()
        {
            return m_state.ExitMatchmaking();
        }
        public Task JoinLobby(string code)
        {
            return m_state.JoinLobby(code);
        }
        public Task LeaveLobby()
        {
            return m_state.LeaveLobby();
        }
        public Task StartGame()
        {
            return m_state.StartGame();
        }
        public Task ExitGame()
        {
            return m_state.ExitGame();
        }

        public Task WriteChatMessage(string message)
        {
            return m_state.WriteChatMessage(message);
        }

        public LobbyPlayerDto ToDto(bool isHost)
        {
            return new LobbyPlayerDto(ApplicationUser.UserName ?? string.Empty, isHost);
        }

        private PlayerState m_state;

    }
}
