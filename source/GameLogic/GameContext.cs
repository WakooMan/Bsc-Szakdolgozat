using GameLogic.Elements;
using GameLogic.Elements.Developments;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Military;
using GameLogic.Elements.Modifiers;
using GameLogic.Elements.Wonders;
using GameLogic.Events;
using GameLogic.Handlers;
using GameLogic.Interfaces;
using Microsoft.Extensions.DependencyInjection;
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
        public IRandomGenerator RandomGenerator { get; }

        public ICardList? CardList { get; private set; }
        public ICardList? DroppedCardList { get; private set; }

        public IWonderList? WonderList { get; private set; }

        public IDevelopmentList? DevelopmentList { get; private set; }
        public IMilitaryBoard? MilitaryBoard { get; private set; }

        public GameContext(IAgeHandler ageHandler, ITurnHandler turnHandler, IPlayerActionReceiver playerActionReceiver, IEventManager eventManager, ICostCalculator costCalculator, IChooseWonderHandler chooseWonderHandler, IGameElements gameElements, IRandomGenerator randomGenerator, [FromKeyedServices(nameof(EmptyCardListFactory))] ICardListFactory droppedCardListFactory, IMilitaryBoardFactory militaryBoardFactory, IRandomElementReceiver randomElementReceiver)
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
            ArgumentChecker.CheckNull(randomElementReceiver, nameof(randomElementReceiver));

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
            m_randomElementReceiver = randomElementReceiver;
        }

        public void Initialize(ICollection<Player> players)
        {
            CardList = m_gameElements.Cards;
            WonderList = m_gameElements.Wonders;
            DevelopmentList = m_gameElements.Developments;
            DroppedCardList = m_droppedCardListFactory.Create();
            MilitaryBoard = m_militaryBoardFactory.Create();
            ICollection<Wonder> wonders = m_randomElementReceiver.ReceiveRandomElements(WonderList.Wonders, 8);
            WonderList.Wonders.RemoveAll(wonders.Contains);
            ChooseWonderHandler.Initialize(players, wonders, this);
            TurnHandler.Initialize(players);
            EventManager.ClearSubscriptions();
            ICollection<Development> developments = m_randomElementReceiver.ReceiveRandomElements(DevelopmentList.Developments, 3);
            DevelopmentList.Developments.RemoveAll(developments.Contains);
            MilitaryBoard.Initialize(players, developments, this);
        }

        private readonly IGameElements m_gameElements;
        private readonly ICardListFactory m_droppedCardListFactory;
        private readonly IMilitaryBoardFactory m_militaryBoardFactory;
        private readonly IRandomElementReceiver m_randomElementReceiver;
    }
}
