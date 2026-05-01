using SevenWonders.Web.Server.Model.Client;
using SevenWonders.Web.Server.Model.Lobby;
using SevenWonders.Web.Server.Model.PlayerStates.Factories;
using SevenWonders.Web.Server.Model.ServerHub;

namespace SevenWonders.Web.Server.Model.PlayerStates
{
    public abstract class PlayerState
    {
        protected PlayerState(IPlayerClient player, IServerService serverService, IPlayerStateFactory playerStateFactory, ILobbyCodeGenerator lobbyCodeGenerator)
        {
            m_player = player;
            m_serverService = serverService;
            m_playerStateFactory = playerStateFactory;
            m_lobbyCodeGenerator = lobbyCodeGenerator;
        }

        public abstract Task CreateLobby(string name);
        public abstract Task StartMatchmaking();
        public abstract Task ExitMatchmaking();
        public abstract Task JoinLobby(string code);
        public abstract Task LeaveLobby();
        public abstract Task StartGame();
        public abstract Task ExitGame();
        public abstract Task WriteChatMessage(string message);

        protected readonly IPlayerClient m_player;
        protected readonly IServerService m_serverService;
        protected readonly IPlayerStateFactory m_playerStateFactory;
        protected readonly ILobbyCodeGenerator m_lobbyCodeGenerator;
    }
}
