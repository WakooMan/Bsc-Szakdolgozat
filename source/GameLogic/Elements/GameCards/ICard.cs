using GameLogic.Ages;
using GameLogic.Elements.Goods;

namespace GameLogic.Elements.GameCards
{
    public interface ICard
    {
        List<Good> GoodCost { get; set; }
        int MoneyCost { get; set; }
        string Name { get; set; }
        string PreviousBuilding { get; set; }
        AgesEnum Age { get; set; }

        ICard Clone();
    }
}
