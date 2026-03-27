using GameLogic.Elements;
using GameLogic.Elements.Goods;

namespace GameLogic.Handlers
{
    public interface ICostCalculator
    {
        Task<int> GetBuildCost(IBuildable buildable, Player buyer, Player opponent);
        Task<bool> CanAfford(IBuildable buildable, Player buyer, Player opponent);
        List<Good> GetMissingGoods(IBuildable buildable, Player buyer);
    }
}
