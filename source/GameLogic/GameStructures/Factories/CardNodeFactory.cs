using GameLogic.Elements.GameCards;
using SevenWonders.Common;

namespace GameLogic.GameStructures.Factories
{
    public class CardNodeFactory : ICardNodeFactory
    {
        public ICardNode Create(ICard card)
        {
            ArgumentChecker.CheckNull(card, nameof(card));
            return new CardNode(card);
        }
    }
}
