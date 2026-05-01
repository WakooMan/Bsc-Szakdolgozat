using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Goods;

namespace SevenWonders.Game.Logic.Handlers
{
    public interface IBuildable
    {
        List<Good> GoodCost { get; set; }
        void OnBuilt(IGameContext gameContext, Player owner, Player opponent);
        string BuildingType { get; }
        int MoneyCost { get; }
    }
}
