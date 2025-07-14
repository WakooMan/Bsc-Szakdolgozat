namespace GameLogic.Elements.GameCards
{
    public class CardList: ICardList
    {
        public List<ICard> Cards { get; set; }

        public CardList()
        {
            Cards = new List<ICard>();
        }
    }
}
