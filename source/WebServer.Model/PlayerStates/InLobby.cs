using WebServer.Model.Client;
using WebServer.Model.Lobby;
using WebServer.Model.PlayerStates.Factories;

namespace WebServer.Model.PlayerStates
{
    public class InLobby : PlayerState
    {
        public InLobby(IPlayerStateFactory playerStateFactory, ILobbyManager lobbyManager, IPlayerClient player, string lobbyCode) : base(player)
        {
            m_lobbyManager = lobbyManager;
            m_lobbyCode = lobbyCode;
            m_playerStateFactory = playerStateFactory;
        }

        public override ILobby CreateLobby(string code, string name)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
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
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override void LeaveLobby()
        {
            ILobby? lobby = m_lobbyManager.GetLobby(m_lobbyCode);
            if (lobby is null)
            {
                throw new InvalidOperationException("Cannot leave lobby, because it is not found!");
            }

            lobby.RemoveMember(m_player);
            if (lobby.Members.Count < 1)
            {
                m_lobbyManager.RemoveLobby(m_lobbyCode);
            }
            else
            {
                lobby.HostConnectionId = lobby.Members.First().Key;
            }

            m_player.ChangeState(m_playerStateFactory.CreateInMainMenuState(m_player));
        }

        public override void StartGame()
        {
            ILobby? lobby = m_lobbyManager.GetLobby(m_lobbyCode);

            if (lobby is null)
            {
                throw new InvalidOperationException("Cannot start game from lobby, because it is not found!");
            }

            m_lobbyManager.RemoveLobby(m_lobbyCode);
            foreach (var member in lobby.Members)
            {
                member.Value.ChangeState(m_playerStateFactory.CreateInGameState(member.Value));
            }
        }

        public override void StartMatchmaking()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        private readonly string m_lobbyCode;
        private readonly ILobbyManager m_lobbyManager;
        private readonly IPlayerStateFactory m_playerStateFactory;
    }
}
