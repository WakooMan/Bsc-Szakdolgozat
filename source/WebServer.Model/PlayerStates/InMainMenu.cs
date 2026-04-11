using WebServer.Model.Client;
using WebServer.Model.Lobby;
using WebServer.Model.PlayerStates.Factories;

namespace WebServer.Model.PlayerStates
{
    public class InMainMenu : PlayerState
    {
        public InMainMenu(ILobbyManager lobbyManager, IPlayerStateFactory playerStateFactory, IPlayerClient player) : base(player)
        {
            m_lobbyManager = lobbyManager;
            m_playerStateFactory = playerStateFactory;
        }

        public override ILobby CreateLobby(string code, string name)
        {
            ILobby? lobby = m_lobbyManager.GetLobby(code);
            if (lobby is not null)
            {
                throw new InvalidOperationException($"Cannot create lobby, because lobby with code: {code} already exists!");
            }

            if (!m_lobbyManager.AddLobby(m_player.ConnectionId, code, name, out ILobby result))
            {
                throw new InvalidOperationException("Could not add lobby to the lobby manager!");
            }

            m_player.ChangeState(m_playerStateFactory.CreateInLobbyState(m_player, code));
            return result;
        }

        public override void ExitGame()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override void ExitMatchmaking()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override ILobby JoinLobby(string code)
        {
            ILobby? lobby = m_lobbyManager.GetLobby(code);
            if (lobby is null)
            {
                throw new InvalidOperationException($"Lobby with code: {code} does not exist!");
            }

            if (lobby.Members.Count < 2)
            {
                lobby.AddMember(m_player);
                m_player.ChangeState(m_playerStateFactory.CreateInLobbyState(m_player, code));
                return lobby;
            }

            throw new InvalidOperationException("The lobby is filled!");
        }

        public override void LeaveLobby()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override void StartGame()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override void StartMatchmaking()
        {
            m_player.ChangeState(m_playerStateFactory.CreateInMatchmakingState(m_player));
        }

        private readonly ILobbyManager m_lobbyManager;
        private readonly IPlayerStateFactory m_playerStateFactory;
    }
}
