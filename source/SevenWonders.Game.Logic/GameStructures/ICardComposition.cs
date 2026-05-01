namespace SevenWonders.Game.Logic.GameStructures
{
    public interface ICardComposition
    {
        IReadOnlyList<ICardNode> AvailableCards { get; }
        IReadOnlyList<ICardNode> AllCards { get; }
        void RemoveCard(ICardNode card);
    }
}
