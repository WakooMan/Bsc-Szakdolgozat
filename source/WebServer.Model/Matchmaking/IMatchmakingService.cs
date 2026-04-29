using WebServer.Model.Client;
using WebServer.Model.PlayerStates;

namespace WebServer.Model.Matchmaking
{
    public interface IMatchmakingService
    {
        Task AddPlayer(IPlayerClient player);
        Task RemovePlayer(IPlayerClient player);
        bool TryGetMatch(IPlayerClient player, out Match? match);
    }
}
