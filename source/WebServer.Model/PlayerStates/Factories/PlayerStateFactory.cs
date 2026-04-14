using WebServer.Model.Client;
using WebServer.Model.Lobby;
using WebServer.Model.MessageHandling;
using WebServer.Model.ServerHub;

namespace WebServer.Model.PlayerStates.Factories
{
    public class PlayerStateFactory : IPlayerStateFactory
    {
        public PlayerStateFactory(ILobbyManager lobbyManager, IServerService serverService, ILobbyCodeGenerator lobbyCodeGenerator, IGameManager gameManager, IServerMessageDispatcher serverMessageDispatcher)
        {
            m_lobbyManager = lobbyManager;
            m_serverService = serverService;
            m_lobbyCodeGenerator = lobbyCodeGenerator;
            m_gameManager = gameManager;
            m_serverMessageDispatcher = serverMessageDispatcher;
        }

        public InGame CreateInGameState(IPlayerClient playerClient, string gameCode)
        {
            return new InGame(this, playerClient, m_serverService, m_lobbyCodeGenerator, m_gameManager, m_serverMessageDispatcher, gameCode);
        }

        public InLobby CreateInLobbyState(IPlayerClient playerClient, string lobbyCode)
        {
            return new InLobby(this, m_lobbyManager, playerClient, m_serverService, m_lobbyCodeGenerator, m_gameManager, lobbyCode);
        }

        public InMainMenu CreateInMainMenuState(IPlayerClient playerClient)
        {
            return new InMainMenu(m_lobbyManager, this, playerClient, m_serverService, m_lobbyCodeGenerator);
        }

        public InMatchmaking CreateInMatchmakingState(IPlayerClient playerClient)
        {
            return new InMatchmaking(this, playerClient, m_serverService, m_lobbyCodeGenerator);
        }

        private readonly ILobbyManager m_lobbyManager;
        private readonly IServerService m_serverService;
        private readonly ILobbyCodeGenerator m_lobbyCodeGenerator;
        private readonly IGameManager m_gameManager;
        private readonly IServerMessageDispatcher m_serverMessageDispatcher;
    }
}
