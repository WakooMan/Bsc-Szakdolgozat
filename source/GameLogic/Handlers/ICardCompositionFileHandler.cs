using GameLogic.GameStructures;

namespace GameLogic.Handlers
{
    public interface ICardCompositionFileHandler
    {
        void SetCompositionForCards(List<ICardNode> cardNodes);
    }
}
