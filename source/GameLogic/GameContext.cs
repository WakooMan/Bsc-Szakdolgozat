using GameLogic.Elements;
using GameLogic.Elements.Developments;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Military;
using GameLogic.Elements.Modifiers;
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
        public IRandomGenerator RandomGenerator { get; }

        public ICardList? CardList { get; private set; }
        public ICardList? DroppedCardList { get; private set; }

        public IWonderList? WonderList { get; private set; }

        public IDevelopmentList? DevelopmentList { get; private set; }
        public IMilitaryBoard? MilitaryBoard { get; private set; }

        [ImportingConstructor]
        public GameContext(IAgeHandler ageHandler, ITurnHandler turnHandler, IPlayerActionReceiver playerActionReceiver, IEventManager eventManager, ICostCalculator costCalculator, IChooseWonderHandler chooseWonderHandler, IGameElements gameElements, IRandomGenerator randomGenerator, [Import(nameof(EmptyCardListFactory), typeof(ICardListFactory))] ICardListFactory droppedCardListFactory, IMilitaryBoardFactory militaryBoardFactory)
        {
            ArgumentChecker.CheckNull(ageHandler, nameof(ageHandler));
            ArgumentChecker.CheckNull(turnHandler, nameof(turnHandler));
            ArgumentChecker.CheckNull(playerActionReceiver, nameof(playerActionReceiver));
            ArgumentChecker.CheckNull(eventManager, nameof(eventManager));
            ArgumentChecker.CheckNull(costCalculator, nameof(costCalculator));
            ArgumentChecker.CheckNull(chooseWonderHandler, nameof(chooseWonderHandler));
            ArgumentChecker.CheckNull(gameElements, nameof(gameElements));
            ArgumentChecker.CheckNull(randomGenerator, nameof(randomGenerator));
            ArgumentChecker.CheckNull(droppedCardListFactory, nameof(droppedCardListFactory));
            ArgumentChecker.CheckNull(militaryBoardFactory, nameof(militaryBoardFactory));

            AgeHandler = ageHandler;
            TurnHandler = turnHandler;
            PlayerActionReceiver = playerActionReceiver;
            EventManager = eventManager;
            CostCalculator = costCalculator;
            ChooseWonderHandler = chooseWonderHandler;
            m_gameElements = gameElements;
            m_droppedCardListFactory = droppedCardListFactory;
            RandomGenerator = randomGenerator;
            m_militaryBoardFactory = militaryBoardFactory;
        }

        public void Initialize(ICollection<Player> players, ICollection<Wonder> wonders, ICollection<Development> developments)
        {
            CardList = m_gameElements.Cards;
            WonderList = m_gameElements.Wonders;
            DevelopmentList = m_gameElements.Developments;
            DroppedCardList = m_droppedCardListFactory.Create();
            MilitaryBoard = m_militaryBoardFactory.Create();
            ChooseWonderHandler.Initialize(players, wonders);
            TurnHandler.Initialize(players);
            EventManager.ClearSubscriptions();
            AgeHandler.Initialize();
            MilitaryBoard.Initialize(players, developments, EventManager);
        }

        private readonly IGameElements m_gameElements;
        private readonly ICardListFactory m_droppedCardListFactory;
        private readonly IMilitaryBoardFactory m_militaryBoardFactory;
    }
}
