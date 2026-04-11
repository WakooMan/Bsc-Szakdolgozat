using WebServer.Model.Client;
using WebServer.Model.Lobby;

namespace WebServer.Model.PlayerStates
{
    public abstract class PlayerState
    {
        protected PlayerState(IPlayerClient player)
        {
            m_player = player;
        }

        public abstract ILobby CreateLobby(string code, string name);
        public abstract void StartMatchmaking();
        public abstract void ExitMatchmaking();
        public abstract ILobby JoinLobby(string code);
        public abstract void LeaveLobby();
        public abstract void StartGame();
        public abstract void ExitGame();

        protected readonly IPlayerClient m_player;
    }
}
