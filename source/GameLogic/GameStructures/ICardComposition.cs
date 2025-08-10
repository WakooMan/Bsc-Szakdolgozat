namespace GameLogic.GameStructures
{
    public interface ICardComposition
    {
        IReadOnlyList<ICardNode> AvailableCards { get; }
        IReadOnlyList<ICardNode> AllCards { get; }
        void RemoveCard(ICardNode card);
    }
}
