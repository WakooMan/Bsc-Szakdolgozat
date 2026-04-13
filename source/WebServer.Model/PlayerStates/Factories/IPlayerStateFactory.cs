using WebServer.Model.Client;

namespace WebServer.Model.PlayerStates.Factories
{
    public interface IPlayerStateFactory
    {
        InGame CreateInGameState(IPlayerClient playerClient, string gameCode);
        InLobby CreateInLobbyState(IPlayerClient playerClient, string lobbyCode);
        InMainMenu CreateInMainMenuState(IPlayerClient playerClient);
        InMatchmaking CreateInMatchmakingState(IPlayerClient playerClient);
    }
}
