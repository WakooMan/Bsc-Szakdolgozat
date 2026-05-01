using SevenWonders.Game.Logic;
using SevenWonders.AI.Model.Services.Encoders;

namespace SevenWonders.AI.Model.Factories
{
    public class CardCompositionEncoderFactory : ICardCompositionEncoderFactory
    {
        public CardCompositionEncoderFactory(IGame game, IEasyCardNodeEncoder easyCardNodeEncoder, IMediumCardNodeEncoder mediumCardNodeEncoder, IHardCardNodeEncoder hardCardNodeEncoder)
        {
            m_game = game;
            m_easyCardNodeEncoder = easyCardNodeEncoder;
            m_mediumCardNodeEncoder = mediumCardNodeEncoder;
            m_hardCardNodeEncoder = hardCardNodeEncoder;
        }

        public ICardCompositionEncoder CreateEasy()
        {
            return new CardCompositionEncoder(m_game, m_easyCardNodeEncoder);
        }

        public ICardCompositionEncoder CreateMedium()
        {
            return new CardCompositionEncoder(m_game, m_mediumCardNodeEncoder);
        }

        public ICardCompositionEncoder CreateHard()
        {
            return new CardCompositionEncoder(m_game, m_hardCardNodeEncoder);
        }

        private readonly IGame m_game;
        private readonly IEasyCardNodeEncoder m_easyCardNodeEncoder;
        private readonly IMediumCardNodeEncoder m_mediumCardNodeEncoder;
        private readonly IHardCardNodeEncoder m_hardCardNodeEncoder;
    }
}
