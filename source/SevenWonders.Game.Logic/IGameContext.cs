using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Developments;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Elements.Military;
using SevenWonders.Game.Logic.Elements.Modifiers;
using SevenWonders.Game.Logic.Elements.Wonders;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Handlers;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.Common;

namespace SevenWonders.Game.Logic
{
    public interface IGameContext
    {
        public IChooseWonderHandler ChooseWonderHandler { get; }
        public IAgeHandler AgeHandler { get; }
        public ITurnHandler TurnHandler { get; }
        public IEventManager EventManager { get; }
        public ICostCalculator CostCalculator { get; }
        public IRandomGenerator? RandomGenerator { get; }
        public ICardList? CardList { get; }
        public ICardList? DroppedCardList { get; }
        public IWonderList? WonderList { get; }
        public IDevelopmentList? DevelopmentList { get; }
        public IMilitaryBoard? MilitaryBoard { get; }
        public IPlayerActionHandler PlayerActionHandler { get; }
        void Initialize(ICollection<Player> players, IRandomGenerator randomGenerator);
    }
}
