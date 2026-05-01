using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Goods;

namespace SevenWonders.Game.Logic.Handlers
{
    public interface ICostCalculator
    {
        int GetBuildCost(IBuildable buildable, Player buyer, Player opponent);
        bool CanAfford(IBuildable buildable, Player buyer, Player opponent);
        List<Good> GetMissingGoods(IBuildable buildable, PlayerProperties buyerProperties);
    }
}
