namespace GameLogic.Elements.GameCards
{
    public class CardList: ICardList
    {
        public List<Card> Cards { get; set; }

        public CardList()
        {
            Cards = new List<Card>();
        }

        private CardList(CardList cardList)
        {
            Cards = cardList.Cards.Select(card => card.Clone()).ToList();
        }

        public ICardList Clone()
        {
            return new CardList(this);
        }
    }
}
