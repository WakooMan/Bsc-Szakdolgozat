using SevenWonders.Game.Logic.Ages;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Common;

namespace SevenWonders.Game.Logic.Handlers
{
    public interface IAgeHandler
    {
        IAgeBase CurrentAge { get; }
        void Initialize(IRandomGenerator? randomGenerator, ICardList? cards);
        bool NextAge();
    }
}
