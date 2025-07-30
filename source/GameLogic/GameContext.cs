using GameLogic.Elements;
using GameLogic.Elements.Developments;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;
using GameLogic.Events;
using GameLogic.Handlers;
using GameLogic.Interfaces;
using SevenWonders.Common;
using System.ComponentModel.Composition;

namespace GameLogic
{
    [Export(typeof(IGameContext))]
    public class GameContext : IGameContext
    {
        public IAgeHandler AgeHandler { get; }

        public ITurnHandler TurnHandler { get; }

        public IPlayerActionReceiver PlayerActionReceiver { get; }

        public IEventManager EventManager { get; }

        public ICostCalculator CostCalculator { get; }

        public IChooseWonderHandler ChooseWonderHandler { get; }

        public ICardList? CardList { get; private set; }
        public ICardList? DroppedCardList { get; private set; }

        public IWonderList? WonderList { get; private set; }

        public IDevelopmentList? DevelopmentList { get; private set; }
        public IRandomGenerator RandomGenerator { get; }

        [ImportingConstructor]
        public GameContext(IAgeHandler ageHandler, ITurnHandler turnHandler, IPlayerActionReceiver playerActionReceiver, IEventManager eventManager, ICostCalculator costCalculator, IChooseWonderHandler chooseWonderHandler, IGameElements gameElements, IRandomGenerator randomGenerator, [Import(nameof(EmptyCardListFactory), typeof(ICardListFactory))] ICardListFactory droppedCardListFactory)
        {
            AgeHandler = ageHandler;
            TurnHandler = turnHandler;
            PlayerActionReceiver = playerActionReceiver;
            EventManager = eventManager;
            CostCalculator = costCalculator;
            ChooseWonderHandler = chooseWonderHandler;
            m_gameElements = gameElements;
            m_droppedCardListFactory = droppedCardListFactory;
            RandomGenerator = randomGenerator;
        }

        public void Initialize(ICollection<Player> players, ICollection<Wonder> wonders)
        {
            CardList = m_gameElements.Cards;
            WonderList = m_gameElements.Wonders;
            DevelopmentList = m_gameElements.Developments;
            DroppedCardList = m_droppedCardListFactory.Create();
            ChooseWonderHandler.Initialize(players, wonders);
            TurnHandler.Initialize(players);
            EventManager.ClearSubscriptions();
            AgeHandler.Initialize();
        }

        private readonly IGameElements m_gameElements;
        private readonly ICardListFactory m_droppedCardListFactory;
    }
}
