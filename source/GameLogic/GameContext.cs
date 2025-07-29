using GameLogic.Elements;
using GameLogic.Elements.Developments;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;
using GameLogic.Events;
using GameLogic.Handlers;
using GameLogic.Interfaces;
using SevenWonders.Common;

namespace GameLogic
{
    public class GameContext : IGameContext
    {
        public IAgeHandler AgeHandler { get; }

        public ITurnHandler TurnHandler { get; }

        public IPlayerActionReceiver PlayerActionReceiver { get; }

        public IEventManager EventManager { get; }

        public ICostCalculator CostCalculator { get; }

        public IChooseWonderHandler ChooseWonderHandler { get; }

        public ICardList CardList { get; }
        public ICardList DroppedCardList { get; }

        public IWonderList WonderList { get; }

        public IDevelopmentList DevelopmentList { get; }
        public IRandomGenerator RandomGenerator { get; }

        public GameContext(IAgeHandler ageHandler, ITurnHandler turnHandler, IPlayerActionReceiver playerActionReceiver, IEventManager eventManager, ICostCalculator costCalculator, IChooseWonderHandler chooseWonderHandler, IGameElements gameElements, IRandomGenerator randomGenerator, ICardListFactory droppedCardListFactory)
        {
            AgeHandler = ageHandler;
            TurnHandler = turnHandler;
            PlayerActionReceiver = playerActionReceiver;
            EventManager = eventManager;
            CostCalculator = costCalculator;
            ChooseWonderHandler = chooseWonderHandler;
            CardList = gameElements.Cards;
            WonderList = gameElements.Wonders;
            DevelopmentList = gameElements.Developments;
            RandomGenerator = randomGenerator;
            DroppedCardList = droppedCardListFactory.Create();
        }
    }
}
