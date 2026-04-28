using GameLogic.Elements;

namespace SevenWonders.AI.Model.Services.Encoders
{
    public interface ICardCompositionEncoder
    {
        void InitializeComposition();
        void ClearComposition();
        void EncodeNodes(List<float> vector, PlayerProperties ownerProperties, PlayerProperties opponentProperties);
    }
}
