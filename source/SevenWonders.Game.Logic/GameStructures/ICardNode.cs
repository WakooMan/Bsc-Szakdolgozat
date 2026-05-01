using SevenWonders.Game.Logic.Elements.GameCards;

namespace SevenWonders.Game.Logic.GameStructures
{
    public interface ICardNode
    {
        Card CardObj { get; }
        bool Hidden { get; set; }
        string NodeName { get; set; }
        IReadOnlyList<ICardNode> CoveredBy { get; }
        void AddParent(ICardNode cardNode);
        void RemoveParent(ICardNode cardNode);
    }
}
