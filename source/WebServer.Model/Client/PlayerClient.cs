using WebServer.Contract.DataTransferObjects;
using WebServer.Contract.Messages.Lobby.ServerMessages;
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

        public Task<LobbyServerMessage> CreateLobby(string name)
        {
            return m_state.CreateLobby(name);
        }
        public Task<LobbyServerMessage> StartMatchmaking()
        {
            return m_state.StartMatchmaking();
        }
        public Task<LobbyServerMessage> ExitMatchmaking()
        {
            return m_state.ExitMatchmaking();
        }
        public Task<LobbyServerMessage> JoinLobby(string code)
        {
            return m_state.JoinLobby(code);
        }
        public Task<LobbyServerMessage> LeaveLobby()
        {
            return m_state.LeaveLobby();
        }
        public Task<LobbyServerMessage> StartGame()
        {
            return m_state.StartGame();
        }
        public Task<LobbyServerMessage> ExitGame()
        {
            return m_state.ExitGame();
        }

        public Task<LobbyServerMessage> WriteChatMessage(string message)
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
