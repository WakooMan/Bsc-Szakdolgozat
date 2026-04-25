using SevenWonders.AI.Model.Services;
using SevenWonders.AI.Model.Services.Encoders;

namespace SevenWonders.AITrainerServer.Factories
{
    public class GameStateVectorReceiverFactory : IGameStateVectorReceiverFactory
    {
        public GameStateVectorReceiverFactory(ICardCompositionEncoderFactory cardCompositionEncoderFactory, 
                                              IEasyPlayerEncoder easyPlayerEncoder,
                                              IMediumPlayerEncoder mediumPlayerEncoder,
                                              IEasyGlobalInfoEncoder easyGlobalInfoEncoder,
                                              IMediumGlobalInfoEncoder mediumGlobalInfoEncoder)
        {
            m_cardCompositionEncoderFactory = cardCompositionEncoderFactory;
            m_easyPlayerEncoder = easyPlayerEncoder;
            m_easyGlobalInfoEncoder = easyGlobalInfoEncoder;
            m_mediumPlayerEncoder = mediumPlayerEncoder;
            m_mediumGlobalInfoEncoder = mediumGlobalInfoEncoder;
        }

        public IGameStateVectorReceiver CreateEasy()
        {
            return new GameStateVectorReceiver(m_cardCompositionEncoderFactory.CreateEasy(), m_easyPlayerEncoder, m_easyGlobalInfoEncoder);
        }

        public IGameStateVectorReceiver CreateMedium()
        {
            return new GameStateVectorReceiver(m_cardCompositionEncoderFactory.CreateMedium(), m_mediumPlayerEncoder, m_mediumGlobalInfoEncoder);
        }

        private readonly ICardCompositionEncoderFactory m_cardCompositionEncoderFactory;
        private readonly IEasyPlayerEncoder m_easyPlayerEncoder;
        private readonly IEasyGlobalInfoEncoder m_easyGlobalInfoEncoder;
        private readonly IMediumPlayerEncoder m_mediumPlayerEncoder;
        private readonly IMediumGlobalInfoEncoder m_mediumGlobalInfoEncoder;
    }
}
