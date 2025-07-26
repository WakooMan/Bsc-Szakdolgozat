using GameLogic.Elements;
using GameLogic.Elements.Goods;
using GameLogic.Events;

namespace GameLogic.Handlers
{
    public interface IBuildable
    {
        List<Good> GoodCost { get; set; }
        void OnBuilt(Player player, IEventManager eventManager);
    }
}
