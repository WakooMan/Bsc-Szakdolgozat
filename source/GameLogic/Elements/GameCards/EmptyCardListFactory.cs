using System.ComponentModel.Composition;

namespace GameLogic.Elements.GameCards
{
    [Export(nameof(EmptyCardListFactory), typeof(ICardListFactory))]
    public class EmptyCardListFactory : ICardListFactory
    {
        public ICardList Create()
        {
            return new CardList();
        }
    }
}
