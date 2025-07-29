using GameLogic.Elements;
using GameLogic.Elements.Goods;

namespace GameLogic.Handlers
{
    public interface IBuildable
    {
        List<Good> GoodCost { get; set; }
        void OnBuilt(IGameContext gameContext);
    }
}
