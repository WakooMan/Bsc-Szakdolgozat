using GameLogic.Events;
using GameLogic.Handlers;
using GameLogic.Interfaces;

namespace GameLogic
{
    public interface IGameContext
    {
        public IChooseWonderHandler ChooseWonderHandler { get; }
        public IAgeHandler AgeHandler { get; }
        public ITurnHandler TurnHandler { get; }
        public IPlayerActionReceiver PlayerActionReceiver { get; }
        public IEventManager EventManager { get; }
        public ICostCalculator CostCalculator { get; }
    }
}
