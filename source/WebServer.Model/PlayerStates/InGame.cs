using WebServer.Model.Client;
using WebServer.Model.Lobby;
using WebServer.Model.PlayerStates.Factories;

namespace WebServer.Model.PlayerStates
{
    public class InGame : PlayerState
    {
        public InGame(IPlayerStateFactory playerStateFactory, IPlayerClient player) : base(player)
        {
            m_playerStateFactory = playerStateFactory;
        }

        public override ILobby CreateLobby(string code, string name)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override void ExitGame()
        {
            m_player.ChangeState(m_playerStateFactory.CreateInMainMenuState(m_player));
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
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override void StartGame()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override void StartMatchmaking()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        private readonly IPlayerStateFactory m_playerStateFactory;
    }
}
