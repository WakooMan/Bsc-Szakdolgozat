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

        public AgeHandler(ICardCompositionFactory cardCompositionFactory, IGameElements gameElements, IEventManager eventManager)
        {
            ArgumentChecker.CheckNull(cardCompositionFactory, nameof(cardCompositionFactory));
            ArgumentChecker.CheckNull(gameElements, nameof(gameElements));
            ArgumentChecker.CheckNull(eventManager, nameof(eventManager));

            m_cardCompositionFactory = cardCompositionFactory;
            m_cardList = gameElements.Cards;
            m_eventManager = eventManager;
            m_ageBase = null;
        }

        public async Task Initialize(IRandomGenerator? randomGenerator)
        {
            m_randomGenerator = randomGenerator ?? throw new ArgumentNullException(nameof(randomGenerator));
            m_ageBase = new FirstAge(m_eventManager, m_cardCompositionFactory, m_cardList, m_randomGenerator);
            await m_eventManager.PublishAsync(new OnAgeStarted(m_ageBase));
        }

        public async Task<bool> NextAge()
        {
            if (CurrentAge is null)
            {
                throw new InvalidOperationException("Initialize method is not called yet!");
            }

            IAgeBase previousAge = CurrentAge;

            switch (CurrentAge.Age)
            {
                case AgesEnum.I:
                    m_ageBase = new SecondAge(m_eventManager, m_cardCompositionFactory, m_cardList, m_randomGenerator);
                    await m_eventManager.PublishAsync(new OnAgeStarted(m_ageBase));
                    break;
                case AgesEnum.II:
                    m_ageBase = new ThirdAge(m_eventManager, m_cardCompositionFactory, m_cardList, m_randomGenerator);
                    await m_eventManager.PublishAsync(new OnAgeStarted(m_ageBase));
                    break;
                default:
                    return false;
            }

            await m_eventManager.PublishAsync(new OnAgeEnded(previousAge));
            return true;
        }

        private readonly ICardCompositionFactory m_cardCompositionFactory;
        private readonly ICardList m_cardList;
        private readonly IEventManager m_eventManager;
        private IRandomGenerator? m_randomGenerator;
        private IAgeBase? m_ageBase;
    }
}
