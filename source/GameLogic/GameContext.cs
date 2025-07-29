using GameLogic.Events;
using GameLogic.Handlers;
using GameLogic.Interfaces;

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

        public GameContext(IAgeHandler ageHandler, ITurnHandler turnHandler, IPlayerActionReceiver playerActionReceiver, IEventManager eventManager, ICostCalculator costCalculator, IChooseWonderHandler chooseWonderHandler)
        {
            AgeHandler = ageHandler;
            TurnHandler = turnHandler;
            PlayerActionReceiver = playerActionReceiver;
            EventManager = eventManager;
            CostCalculator = costCalculator;
            ChooseWonderHandler = chooseWonderHandler;
        }
    }
}
