using GameLogic.Elements;
using GameLogic.Elements.Goods;

namespace GameLogic.Handlers
{
    public interface IBuildable
    {
        List<Good> GoodCost { get; set; }
        Task OnBuilt(IGameContext gameContext, Player owner, Player opponent);
        string BuildingType { get; }
        int MoneyCost { get; }
    }
}
