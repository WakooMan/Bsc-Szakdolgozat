using GameLogic.Elements.Developments;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;

namespace GameLogic.Elements
{
    public interface IGameElements
    {
        ICardList Cards { get; }
        IWonderList Wonders { get; }
        IDevelopmentList Developments { get; }
    }
}
