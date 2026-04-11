using WebServer.Model.Client;
using WebServer.Model.Lobby;

namespace WebServer.Model.PlayerStates.Factories
{
    public class PlayerStateFactory : IPlayerStateFactory
    {
        public PlayerStateFactory(ILobbyManager lobbyManager)
        {
            m_lobbyManager = lobbyManager;
        }

        public InGame CreateInGameState(IPlayerClient playerClient)
        {
            return new InGame(this, playerClient);
        }

        public InLobby CreateInLobbyState(IPlayerClient playerClient, string lobbyCode)
        {
            return new InLobby(this, m_lobbyManager, playerClient, lobbyCode);
        }

        public InMainMenu CreateInMainMenuState(IPlayerClient playerClient)
        {
            return new InMainMenu(m_lobbyManager, this, playerClient);
        }

        public InMatchmaking CreateInMatchmakingState(IPlayerClient playerClient)
        {
            return new InMatchmaking(this, playerClient);
        }

        private readonly ILobbyManager m_lobbyManager;
    }
}
