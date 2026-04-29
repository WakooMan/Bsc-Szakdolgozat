using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using SevenWonders.Common;

namespace GameLogic.Handlers
{
    public interface IAgeHandler
    {
        IAgeBase CurrentAge { get; }
        void Initialize(IRandomGenerator? randomGenerator, ICardList? cards);
        bool NextAge();
    }
}
