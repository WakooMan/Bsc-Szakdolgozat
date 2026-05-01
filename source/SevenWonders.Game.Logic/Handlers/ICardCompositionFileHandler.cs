using SevenWonders.Game.Logic.GameStructures;

namespace SevenWonders.Game.Logic.Handlers
{
    public interface ICardCompositionFileHandler
    {
        void SetCompositionForCards(List<ICardNode> cardNodes);
    }
}
