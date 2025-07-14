using GameLogic.Elements.GameCards;

namespace GameLogic.GameStructures
{
    public interface ICardNode
    {
        ICard CardObj { get; }
        bool Hidden { get; set; }
        IReadOnlyList<ICardNode> CoveredBy { get; }
        void AddParent(ICardNode cardNode);
        void RemoveParent(ICardNode cardNode);

    }
}
