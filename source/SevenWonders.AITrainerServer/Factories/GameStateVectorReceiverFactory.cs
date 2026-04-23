using SevenWonders.AI.Model.Services;
using SevenWonders.AI.Model.Services.Encoders;

namespace SevenWonders.AITrainerServer.Factories
{
    public class GameStateVectorReceiverFactory : IGameStateVectorReceiverFactory
    {
        public GameStateVectorReceiverFactory(ICardCompositionEncoderFactory cardCompositionEncoderFactory, IPlayerEncoder playerEncoder, IGlobalInfoEncoder globalInfoEncoder)
        {
            m_cardCompositionEncoderFactory = cardCompositionEncoderFactory;
            m_playerEncoder = playerEncoder;
            m_globalInfoEncoder = globalInfoEncoder;
        }

        public IGameStateVectorReceiver Create()
        {
            return new GameStateVectorReceiver(m_cardCompositionEncoderFactory.Create(), m_playerEncoder, m_globalInfoEncoder);
        }

        private readonly ICardCompositionEncoderFactory m_cardCompositionEncoderFactory;
        private readonly IPlayerEncoder m_playerEncoder;
        private readonly IGlobalInfoEncoder m_globalInfoEncoder;
    }
}
