using SevenWonders.AI.Model.Services.Encoders;

namespace SevenWonders.AITrainerServer.Factories
{
    public interface ICardCompositionEncoderFactory
    {
        ICardCompositionEncoder Create();
    }
}
