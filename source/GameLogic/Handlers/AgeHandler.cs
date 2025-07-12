using GameLogic.Ages;
using GameLogic.Handlers.Factories;

namespace GameLogic.Handlers
{
    public class AgeHandler
    {
        private ICardCompositionFileHandlerFactory CardCompositionFileHandlerFactory { get; }

        public AgeBase CurrentAge { get; private set; }

        public delegate void AgeChangedEventHandler(AgesEnum age);

        public event AgeChangedEventHandler HandleAgeChanged;

        public AgeHandler(ICardCompositionFileHandlerFactory cardCompositionFileHandlerFactory)
        {
            CardCompositionFileHandlerFactory = cardCompositionFileHandlerFactory;
            CurrentAge = new FirstAge(CardCompositionFileHandlerFactory);
        }

        public bool NextAge()
        {
            switch (CurrentAge.Age)
            {
                case AgesEnum.I:
                    CurrentAge = new SecondAge(CardCompositionFileHandlerFactory);
                    break;
                case AgesEnum.II:
                    CurrentAge = new ThirdAge(CardCompositionFileHandlerFactory);
                    break;
                default:
                    return false;
            }

            HandleAgeChanged?.Invoke(CurrentAge.Age);
            return true;
        }
    }
}
