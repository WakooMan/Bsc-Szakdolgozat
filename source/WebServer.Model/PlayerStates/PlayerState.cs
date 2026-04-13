using WebServer.Contract.Messages.Lobby.ServerMessages;
using WebServer.Model.Client;
using WebServer.Model.Lobby;
using WebServer.Model.PlayerStates.Factories;
using WebServer.Model.ServerHub;

namespace WebServer.Model.PlayerStates
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

        public abstract Task<LobbyServerMessage> CreateLobby(string name);
        public abstract Task<LobbyServerMessage> StartMatchmaking();
        public abstract Task<LobbyServerMessage> ExitMatchmaking();
        public abstract Task<LobbyServerMessage> JoinLobby(string code);
        public abstract Task<LobbyServerMessage> LeaveLobby();
        public abstract Task<LobbyServerMessage> StartGame();
        public abstract Task<LobbyServerMessage> ExitGame();
        public abstract Task<LobbyServerMessage> WriteChatMessage(string message);

        protected readonly IPlayerClient m_player;
        protected readonly IServerService m_serverService;
        protected readonly IPlayerStateFactory m_playerStateFactory;
        protected readonly ILobbyCodeGenerator m_lobbyCodeGenerator;
    }
}
