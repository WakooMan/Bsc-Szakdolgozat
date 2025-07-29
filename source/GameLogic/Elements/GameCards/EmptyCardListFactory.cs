namespace GameLogic.Elements.GameCards
{
    public class EmptyCardListFactory : ICardListFactory
    {
        public ICardList Create()
        {
            return new CardList();
        }
    }
}
