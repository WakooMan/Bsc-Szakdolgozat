using Microsoft.Extensions.DependencyInjection;
using SevenWonders.Common;
using WebServer.Model.Client;
using WebServer.Model.Lobby;
using WebServer.Model.Matchmaking;
using WebServer.Model.MessageHandling;
using WebServer.Model.ServerHub;

namespace WebServer.Model.PlayerStates.Factories
{
    public class PlayerStateFactory : IPlayerStateFactory
    {
        public PlayerStateFactory(ILobbyManager lobbyManager, 
                                  IServerService serverService, 
                                  ILobbyCodeGenerator lobbyCodeGenerator, 
                                  IGameManager gameManager, 
                                  IServerMessageDispatcher serverMessageDispatcher,
                                  IRandomGeneratorFactory randomGeneratorFactory,
                                  IMatchmakingService matchmakingService,
                                  IServiceScopeFactory serviceScopeFactory)
        {
            m_lobbyManager = lobbyManager;
            m_serverService = serverService;
            m_lobbyCodeGenerator = lobbyCodeGenerator;
            m_gameManager = gameManager;
            m_serverMessageDispatcher = serverMessageDispatcher;
            m_randomGeneratorFactory = randomGeneratorFactory;
            m_matchmakingService = matchmakingService;
            m_serviceScopeFactory = serviceScopeFactory;
        }

        public InGame CreateInGameState(IPlayerClient playerClient, string gameCode)
        {
            return new InGame(this, playerClient, m_serverService, m_lobbyCodeGenerator, m_gameManager, m_serverMessageDispatcher, m_lobbyManager, gameCode);
        }

        public InLobby CreateInLobbyState(IPlayerClient playerClient, string lobbyCode)
        {
            return new InLobby(this, m_lobbyManager, playerClient, m_serverService, m_lobbyCodeGenerator, m_gameManager, m_randomGeneratorFactory, lobbyCode);
        }

        public InMainMenu CreateInMainMenuState(IPlayerClient playerClient)
        {
            return new InMainMenu(m_lobbyManager, this, playerClient, m_serverService, m_lobbyCodeGenerator, m_matchmakingService);
        }

        public InMatchmaking CreateInMatchmakingState(IPlayerClient playerClient)
        {
            return new InMatchmaking(this, playerClient, m_serverService, m_lobbyCodeGenerator, m_matchmakingService, m_randomGeneratorFactory, m_serviceScopeFactory, m_gameManager);
        }

        private readonly ILobbyManager m_lobbyManager;
        private readonly IServerService m_serverService;
        private readonly ILobbyCodeGenerator m_lobbyCodeGenerator;
        private readonly IGameManager m_gameManager;
        private readonly IServerMessageDispatcher m_serverMessageDispatcher;
        private readonly IRandomGeneratorFactory m_randomGeneratorFactory;
        private readonly IMatchmakingService m_matchmakingService;
        private readonly IServiceScopeFactory m_serviceScopeFactory;
    }
}
