namespace GameLogic.Elements.GameCards
{
    public interface ICardList
    {
        List<ICard> Cards { get; }

        ICardList Clone();
    }
}
