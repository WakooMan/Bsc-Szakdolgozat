using GameLogic.Elements.GameCards;

namespace GameLogic.GameStructures.Factories
{
    public interface ICardCompositionFactory
    {
        public ICardComposition Create(string cardCompositionFile, ICollection<ICard> cards);
    }
}
