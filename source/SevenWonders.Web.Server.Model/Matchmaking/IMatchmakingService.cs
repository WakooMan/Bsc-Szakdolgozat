using SevenWonders.Web.Server.Model.Client;
using SevenWonders.Web.Server.Model.PlayerStates;

namespace SevenWonders.Web.Server.Model.Matchmaking
{
    public interface IMatchmakingService
    {
        Task AddPlayer(IPlayerClient player);
        Task RemovePlayer(IPlayerClient player);
        bool TryGetMatch(IPlayerClient player, out Match? match);
    }
}
