using WebServer.Model.Client;
using WebServer.Model.Lobby;
using WebServer.Model.PlayerStates.Factories;

namespace WebServer.Model.PlayerStates
{
    public class InMatchmaking : PlayerState
    {
        public InMatchmaking(IPlayerStateFactory playerStateFactory, IPlayerClient player) : base(player)
        {
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
            m_player.ChangeState(m_playerStateFactory.CreateInMainMenuState(m_player));
        }

        public override ILobby JoinLobby(string code)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override void LeaveLobby()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override void StartGame()
        {
            m_player.ChangeState(m_playerStateFactory.CreateInGameState(m_player));
        }

        public override void StartMatchmaking()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        private readonly IPlayerStateFactory m_playerStateFactory;
    }
}
