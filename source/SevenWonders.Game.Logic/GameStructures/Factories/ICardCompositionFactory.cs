using SevenWonders.Game.Logic.Elements.GameCards;

namespace SevenWonders.Game.Logic.GameStructures.Factories
{
    public interface ICardCompositionFactory
    {
        public ICardComposition Create(string cardCompositionFile, ICollection<Card> cards);
    }
}
