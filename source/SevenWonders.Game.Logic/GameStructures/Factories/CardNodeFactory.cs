using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Common;

namespace SevenWonders.Game.Logic.GameStructures.Factories
{
    public class CardNodeFactory : ICardNodeFactory
    {
        public ICardNode Create(Card card)
        {
            ArgumentChecker.CheckNull(card, nameof(card));
            return new CardNode(card);
        }
    }
}
