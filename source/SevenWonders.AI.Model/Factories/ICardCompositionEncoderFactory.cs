using SevenWonders.AI.Model.Services.Encoders;

namespace SevenWonders.AI.Model.Factories
{
    public interface ICardCompositionEncoderFactory
    {
        ICardCompositionEncoder CreateEasy();
        ICardCompositionEncoder CreateMedium();
        ICardCompositionEncoder CreateHard();
    }
}
