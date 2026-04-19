using GameLogic.Elements;

namespace SevenWonders.AI.Model.Services.Encoders
{
    public interface IPlayerEncoder
    {
        void EncodePlayer(List<float> vector, PlayerProperties playerProperties);
    }
}
