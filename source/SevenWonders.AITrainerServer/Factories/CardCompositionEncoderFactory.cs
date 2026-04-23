using GameLogic;
using SevenWonders.AI.Model.Services.Encoders;

namespace SevenWonders.AITrainerServer.Factories
{
    public class CardCompositionEncoderFactory : ICardCompositionEncoderFactory
    {
        public CardCompositionEncoderFactory(IGame game, ICardNodeEncoder cardNodeEncoder)
        {
            m_game = game;
            m_cardNodeEncoder = cardNodeEncoder;
        }

        public ICardCompositionEncoder Create()
        {
            return new CardCompositionEncoder(m_game, m_cardNodeEncoder);
        }

        private readonly IGame m_game;
        private readonly ICardNodeEncoder m_cardNodeEncoder;
    }
}
