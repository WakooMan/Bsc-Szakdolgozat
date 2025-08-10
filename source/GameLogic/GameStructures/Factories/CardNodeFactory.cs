using GameLogic.Elements.GameCards;
using SevenWonders.Common;
using System.ComponentModel.Composition;

namespace GameLogic.GameStructures.Factories
{
    [Export(typeof(ICardNodeFactory))]
    public class CardNodeFactory : ICardNodeFactory
    {
        public ICardNode Create(Card card)
        {
            ArgumentChecker.CheckNull(card, nameof(card));
            return new CardNode(card);
        }
    }
}
