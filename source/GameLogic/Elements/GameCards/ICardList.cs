namespace GameLogic.Elements.GameCards
{
    public interface ICardList
    {
        List<Card> Cards { get; }

        ICardList Clone();
    }
}
