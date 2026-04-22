using GameLogic.Elements;
using GameLogic.Elements.Developments;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Military;
using GameLogic.Elements.Modifiers;
using GameLogic.Elements.Wonders;
using GameLogic.Events;
using GameLogic.Handlers;
using Microsoft.Extensions.DependencyInjection;
using SevenWonders.Common;

namespace GameLogic
{
    public class GameContext : IGameContext
    {
        public IAgeHandler AgeHandler { get; }

        public ITurnHandler TurnHandler { get; }
        public IEventManager EventManager { get; }

        public ICostCalculator CostCalculator { get; }

        public IChooseWonderHandler ChooseWonderHandler { get; }
        public IPlayerActionHandler PlayerActionHandler { get; }

        public ICardList? CardList { get; private set; }
        public ICardList? DroppedCardList { get; private set; }

        public IWonderList? WonderList { get; private set; }

        public IDevelopmentList? DevelopmentList { get; private set; }
        public IMilitaryBoard? MilitaryBoard { get; private set; }
        public IRandomGenerator? RandomGenerator { get; private set; }

        public GameContext(IAgeHandler ageHandler, 
                           ITurnHandler turnHandler,
                           IEventManager eventManager,
                           ICostCalculator costCalculator,
                           IChooseWonderHandler chooseWonderHandler,
                           IGameElements gameElements,
                           [FromKeyedServices(nameof(EmptyCardListFactory))] ICardListFactory droppedCardListFactory,
                           IMilitaryBoardFactory militaryBoardFactory,
                           IPlayerActionHandler playerActionHandler)
        {
            ArgumentChecker.CheckNull(ageHandler, nameof(ageHandler));
            ArgumentChecker.CheckNull(turnHandler, nameof(turnHandler));
            ArgumentChecker.CheckNull(eventManager, nameof(eventManager));
            ArgumentChecker.CheckNull(costCalculator, nameof(costCalculator));
            ArgumentChecker.CheckNull(chooseWonderHandler, nameof(chooseWonderHandler));
            ArgumentChecker.CheckNull(gameElements, nameof(gameElements));
            ArgumentChecker.CheckNull(droppedCardListFactory, nameof(droppedCardListFactory));
            ArgumentChecker.CheckNull(militaryBoardFactory, nameof(militaryBoardFactory));
            ArgumentChecker.CheckNull(playerActionHandler, nameof(playerActionHandler));

            AgeHandler = ageHandler;
            TurnHandler = turnHandler;
            EventManager = eventManager;
            CostCalculator = costCalculator;
            ChooseWonderHandler = chooseWonderHandler;
            m_gameElements = gameElements;
            m_droppedCardListFactory = droppedCardListFactory;
            m_militaryBoardFactory = militaryBoardFactory;
            PlayerActionHandler = playerActionHandler;
        }

        public void Initialize(ICollection<Player> players, IRandomGenerator randomGenerator)
        {
            m_gameElements.ResetElements();
            CardList = m_gameElements.Cards;
            WonderList = m_gameElements.Wonders;
            DevelopmentList = m_gameElements.Developments;
            DroppedCardList = m_droppedCardListFactory.Create();
            MilitaryBoard = m_militaryBoardFactory.Create();
            RandomGenerator = randomGenerator;
            TurnHandler.Initialize(players);
            EventManager.ClearSubscriptions();
        }

        private readonly IGameElements m_gameElements;
        private readonly ICardListFactory m_droppedCardListFactory;
        private readonly IMilitaryBoardFactory m_militaryBoardFactory;
    }
}
