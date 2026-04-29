using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.GameStructures.Factories;
using SevenWonders.Common;

namespace GameLogic.Handlers
{
    public class AgeHandler : IAgeHandler
    {
        public IAgeBase CurrentAge
        {
            get
            {
                if (m_ageBase is null)
                {
                    throw new InvalidOperationException("Cannot get Current Age, because Initialize method is not called yet!");
                }
                return m_ageBase;
            }
        }

        public AgeHandler(ICardCompositionFactory cardCompositionFactory, IEventManager eventManager)
        {
            ArgumentChecker.CheckNull(cardCompositionFactory, nameof(cardCompositionFactory));
            ArgumentChecker.CheckNull(eventManager, nameof(eventManager));

            m_cardCompositionFactory = cardCompositionFactory;
            m_eventManager = eventManager;
            m_ageBase = null;
            m_cardList = null;
        }

        public void Initialize(IRandomGenerator? randomGenerator, ICardList? cards)
        {
            GameLog.Info("Initializing with FirstAge.");
            m_randomGenerator = randomGenerator ?? throw new ArgumentNullException(nameof(randomGenerator));
            m_cardList = cards ?? throw new ArgumentNullException(nameof(cards));
            m_ageBase = new FirstAge(m_eventManager, m_cardCompositionFactory, m_cardList, m_randomGenerator);
            m_eventManager.Publish(new OnAgeStarted(m_ageBase));
        }

        public bool NextAge()
        {
            if (CurrentAge is null || m_cardList is null || m_randomGenerator is null)
            {
                throw new InvalidOperationException("Initialize method is not called yet!");
            }

            IAgeBase previousAge = CurrentAge;

            switch (CurrentAge.Age)
            {
                case AgesEnum.I:
                    GameLog.Info("Transitioning from Age I to Age II.");
                    m_ageBase = new SecondAge(m_eventManager, m_cardCompositionFactory, m_cardList, m_randomGenerator);
                    m_eventManager.Publish(new OnAgeStarted(m_ageBase));
                    break;
                case AgesEnum.II:
                    GameLog.Info("Transitioning from Age II to Age III.");
                    m_ageBase = new ThirdAge(m_eventManager, m_cardCompositionFactory, m_cardList, m_randomGenerator);
                    m_eventManager.Publish(new OnAgeStarted(m_ageBase));
                    break;
                default:
                    GameLog.Info("No more ages to transition to.");
                    return false;
            }

            m_eventManager.Publish(new OnAgeEnded(previousAge));
            return true;
        }

        private readonly ICardCompositionFactory m_cardCompositionFactory;
        private readonly IEventManager m_eventManager;
        private IRandomGenerator? m_randomGenerator;
        private IAgeBase? m_ageBase;
        private ICardList? m_cardList;
    }
}
