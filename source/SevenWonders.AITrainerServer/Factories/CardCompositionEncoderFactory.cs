using GameLogic;
using SevenWonders.AI.Model.Services.Encoders;

namespace SevenWonders.AITrainerServer.Factories
{
    public class CardCompositionEncoderFactory : ICardCompositionEncoderFactory
    {
        public CardCompositionEncoderFactory(IGame game, IEasyCardNodeEncoder easyCardNodeEncoder, IMediumCardNodeEncoder mediumCardNodeEncoder)
        {
            m_game = game;
            m_easyCardNodeEncoder = easyCardNodeEncoder;
            m_mediumCardNodeEncoder = mediumCardNodeEncoder;
        }

        public ICardCompositionEncoder CreateEasy()
        {
            return new CardCompositionEncoder(m_game, m_easyCardNodeEncoder);
        }

        public ICardCompositionEncoder CreateMedium()
        {
            return new CardCompositionEncoder(m_game, m_mediumCardNodeEncoder);
        }

        private readonly IGame m_game;
        private readonly IEasyCardNodeEncoder m_easyCardNodeEncoder;
        private readonly IMediumCardNodeEncoder m_mediumCardNodeEncoder;
    }
}
