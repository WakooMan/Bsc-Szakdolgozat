using GameLogic.Elements;
using GameLogic.Elements.Goods;

namespace GameLogic.Handlers
{
    public interface ICostCalculator
    {
        int GetBuildCost(IBuildable buildable, Player buyer, Player opponent);
        bool CanAfford(IBuildable buildable, Player buyer, Player opponent);
        List<Good> GetMissingGoods(IBuildable buildable, Player buyer);
    }
}
