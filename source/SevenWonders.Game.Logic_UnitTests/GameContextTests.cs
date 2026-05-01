using NSubstitute;
using SevenWonders.Common;
using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Developments;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Elements.Military;
using SevenWonders.Game.Logic.Elements.Modifiers;
using SevenWonders.Game.Logic.Elements.Wonders;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Handlers;

namespace GameLogic_UnitTests
{
    public class GameContextTests
    {
        [SetUp]
        public void Setup()
        {
            m_ageHandler = Substitute.For<IAgeHandler>();
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_eventManager = Substitute.For<IEventManager>();
            m_costCalculator = Substitute.For<ICostCalculator>();
            m_chooseWonderHandler = Substitute.For<IChooseWonderHandler>();
            m_gameElements = Substitute.For<IGameElements>();
            m_droppedCardListFactory = Substitute.For<ICardListFactory>();
            m_militaryBoardFactory = Substitute.For<IMilitaryBoardFactory>();
            m_randomGenerator = Substitute.For<IRandomGenerator>();
            m_wonderList = Substitute.For<IWonderList>();
            m_developmentList = Substitute.For<IDevelopmentList>();
            m_playerActionHandler = Substitute.For<IPlayerActionHandler>();
            m_gameContext = new GameContext(m_ageHandler, m_turnHandler, m_eventManager, m_costCalculator, m_chooseWonderHandler, m_gameElements, m_droppedCardListFactory, m_militaryBoardFactory, m_playerActionHandler);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new GameContext(null, m_turnHandler, m_eventManager, m_costCalculator, m_chooseWonderHandler, m_gameElements, m_droppedCardListFactory, m_militaryBoardFactory, m_playerActionHandler));
            Assert.Throws<ArgumentNullException>(() => new GameContext(m_ageHandler, null, m_eventManager, m_costCalculator, m_chooseWonderHandler, m_gameElements, m_droppedCardListFactory, m_militaryBoardFactory, m_playerActionHandler));
            Assert.Throws<ArgumentNullException>(() => new GameContext(m_ageHandler, m_turnHandler, null, m_costCalculator, m_chooseWonderHandler, m_gameElements, m_droppedCardListFactory, m_militaryBoardFactory, m_playerActionHandler));
            Assert.Throws<ArgumentNullException>(() => new GameContext(m_ageHandler, m_turnHandler, m_eventManager, null, m_chooseWonderHandler, m_gameElements, m_droppedCardListFactory, m_militaryBoardFactory, m_playerActionHandler));
            Assert.Throws<ArgumentNullException>(() => new GameContext(m_ageHandler, m_turnHandler, m_eventManager, m_costCalculator, null, m_gameElements, m_droppedCardListFactory, m_militaryBoardFactory, m_playerActionHandler));
            Assert.Throws<ArgumentNullException>(() => new GameContext(m_ageHandler, m_turnHandler, m_eventManager, m_costCalculator, m_chooseWonderHandler, null, m_droppedCardListFactory, m_militaryBoardFactory, m_playerActionHandler));
            Assert.Throws<ArgumentNullException>(() => new GameContext(m_ageHandler, m_turnHandler, m_eventManager, m_costCalculator, m_chooseWonderHandler, m_gameElements, null, m_militaryBoardFactory, m_playerActionHandler));
            Assert.Throws<ArgumentNullException>(() => new GameContext(m_ageHandler, m_turnHandler, m_eventManager, m_costCalculator, m_chooseWonderHandler, m_gameElements, m_droppedCardListFactory, null, m_playerActionHandler));
            Assert.Throws<ArgumentNullException>(() => new GameContext(m_ageHandler, m_turnHandler, m_eventManager, m_costCalculator, m_chooseWonderHandler, m_gameElements, m_droppedCardListFactory, m_militaryBoardFactory, null));
        }

        [Test]
        public void When_Initialize_Called()
        {
            IMilitaryBoard militaryBoard = Substitute.For<IMilitaryBoard>();
            m_militaryBoardFactory.Create().Returns(militaryBoard);
            m_wonderList.Wonders.Returns([]);
            m_developmentList.Developments.Returns([]);
            m_gameElements.Developments.Returns(m_developmentList);
            m_gameElements.Wonders.Returns(m_wonderList);

            m_gameContext.Initialize([], m_randomGenerator);

            m_militaryBoardFactory.Received(1).Create();
            m_droppedCardListFactory.Received(1).Create();
            _ = m_gameElements.Received(1).Cards;
            _ = m_gameElements.Received(1).Wonders;
            _ = m_gameElements.Received(1).Developments;
            m_turnHandler.Received(1).Initialize(Arg.Any<ICollection<Player>>());
            m_eventManager.Received(1).ClearSubscriptions();
            Assert.That(m_gameContext.AgeHandler, Is.EqualTo(m_ageHandler));
            Assert.That(m_gameContext.TurnHandler, Is.EqualTo(m_turnHandler));
            Assert.That(m_gameContext.EventManager, Is.EqualTo(m_eventManager));
            Assert.That(m_gameContext.CostCalculator, Is.EqualTo(m_costCalculator));
            Assert.That(m_gameContext.ChooseWonderHandler, Is.EqualTo(m_chooseWonderHandler));
            Assert.That(m_gameContext.RandomGenerator, Is.EqualTo(m_randomGenerator));
        }

        [Test]
        public void When_Initialize_Not_Called()
        {
            Assert.That(m_gameContext.DevelopmentList, Is.Null);
            Assert.That(m_gameContext.CardList, Is.Null);
            Assert.That(m_gameContext.DroppedCardList, Is.Null);
            Assert.That(m_gameContext.WonderList, Is.Null);
            Assert.That(m_gameContext.MilitaryBoard, Is.Null);
        }

        private IAgeHandler m_ageHandler;
        private ITurnHandler m_turnHandler;
        private IEventManager m_eventManager;
        private ICostCalculator m_costCalculator;
        private IChooseWonderHandler m_chooseWonderHandler;
        private IGameElements m_gameElements;
        private IRandomGenerator m_randomGenerator;
        private ICardListFactory m_droppedCardListFactory;
        private IMilitaryBoardFactory m_militaryBoardFactory;
        private IWonderList m_wonderList;
        private IDevelopmentList m_developmentList;
        private IPlayerActionHandler m_playerActionHandler;
        private GameContext m_gameContext;
    }
}
