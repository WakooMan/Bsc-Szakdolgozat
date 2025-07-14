namespace GameLogic.GameStructures
{
    public interface ICardComposition
    {
        IReadOnlyList<ICardNode> AvailableCards { get; }
        void RemoveCard(ICardNode card);
    }
}
