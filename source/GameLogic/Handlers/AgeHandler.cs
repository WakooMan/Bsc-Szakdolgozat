using GameLogic.Ages;
using GameLogic.Elements.GameCards;
using GameLogic.GameStructures.Factories;
using SevenWonders.Common;
using static GameLogic.Handlers.IAgeHandler;

namespace GameLogic.Handlers
{
    public class AgeHandler : IAgeHandler
    {
        private ICardCompositionFactory m_cardCompositionFactory;
        private ICardList m_cardList;

        public IAgeBase CurrentAge { get; private set; }
        public event AgeChangedEventHandler HandleAgeChanged;

        public AgeHandler(ICardCompositionFactory cardCompositionFactory, ICardList cardList)
        {
            ArgumentChecker.CheckNull(cardCompositionFactory, nameof(cardCompositionFactory));
            ArgumentChecker.CheckNull(cardList, nameof(cardList));

            m_cardCompositionFactory = cardCompositionFactory;
            m_cardList = cardList;
            CurrentAge = new FirstAge(m_cardCompositionFactory, m_cardList);
        }

        public bool NextAge()
        {
            switch (CurrentAge.Age)
            {
                case AgesEnum.I:
                    CurrentAge = new SecondAge(m_cardCompositionFactory, m_cardList);
                    break;
                case AgesEnum.II:
                    CurrentAge = new ThirdAge(m_cardCompositionFactory, m_cardList);
                    break;
                default:
                    return false;
            }

            HandleAgeChanged?.Invoke(CurrentAge.Age);
            return true;
        }
    }
}
