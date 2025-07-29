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
    public interface IGameContext
    {
        public IChooseWonderHandler ChooseWonderHandler { get; }
        public IAgeHandler AgeHandler { get; }
        public ITurnHandler TurnHandler { get; }
        public IPlayerActionReceiver PlayerActionReceiver { get; }
        public IEventManager EventManager { get; }
        public ICostCalculator CostCalculator { get; }
        public ICardList CardList { get; }
        public ICardList DroppedCardList { get; }
        public IWonderList WonderList { get; }
        public IDevelopmentList DevelopmentList { get; }
        public IRandomGenerator RandomGenerator { get; }
    }
}
