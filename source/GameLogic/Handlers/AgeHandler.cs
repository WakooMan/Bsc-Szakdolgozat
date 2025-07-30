using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.GameStructures.Factories;
using SevenWonders.Common;
using System.ComponentModel.Composition;

namespace GameLogic.Handlers
{
    [Export(typeof(IAgeHandler))]
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

        [ImportingConstructor]
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

        public void Initialize()
        {
            m_ageBase = new FirstAge(m_cardCompositionFactory, m_cardList);
        }

        public bool NextAge()
        {
            if (CurrentAge is null)
            {
                throw new InvalidOperationException("Initialize method is not called yet!");
            }

            AgesEnum previousAge = CurrentAge.Age;

            switch (CurrentAge.Age)
            {
                case AgesEnum.I:
                    m_ageBase = new SecondAge(m_cardCompositionFactory, m_cardList);
                    break;
                case AgesEnum.II:
                    m_ageBase = new ThirdAge(m_cardCompositionFactory, m_cardList);
                    break;
                default:
                    return false;
            }

            m_eventManager.Publish(GameEventType.AgeEnded, new OnAgeEnded(previousAge));
            return true;
        }

        private readonly ICardCompositionFactory m_cardCompositionFactory;
        private readonly ICardList m_cardList;
        private readonly IEventManager m_eventManager;
        private IAgeBase? m_ageBase;
    }
}
