using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Developments;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Military;
using GameLogic.Elements.Modifiers;
using GameLogic.Elements.Wonders;
using GameLogic.Events;
using GameLogic.Handlers;
using GameLogic.Interfaces;
using NSubstitute;
using SevenWonders.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests
{
    public class GameContextTests
    {
        [SetUp]
        public void Setup()
        {
            m_ageHandler = Substitute.For<IAgeHandler>();
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_playerActionReceiver = Substitute.For<IPlayerActionReceiver>();
            m_eventManager = Substitute.For<IEventManager>();
            m_costCalculator = Substitute.For<ICostCalculator>();
            m_chooseWonderHandler = Substitute.For<IChooseWonderHandler>();
            m_gameElements = Substitute.For<IGameElements>();
            m_randomGenerator = Substitute.For<IRandomGenerator>();
            m_droppedCardListFactory = Substitute.For<ICardListFactory>();
            m_militaryBoardFactory = Substitute.For<IMilitaryBoardFactory>();
            m_gameContext = new GameContext(m_ageHandler, m_turnHandler, m_playerActionReceiver, m_eventManager, m_costCalculator, m_chooseWonderHandler, m_gameElements, m_randomGenerator, m_droppedCardListFactory, m_militaryBoardFactory);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new GameContext(null, m_turnHandler, m_playerActionReceiver, m_eventManager, m_costCalculator, m_chooseWonderHandler, m_gameElements, m_randomGenerator, m_droppedCardListFactory, m_militaryBoardFactory));
            Assert.Throws<ArgumentNullException>(() => new GameContext(m_ageHandler, null, m_playerActionReceiver, m_eventManager, m_costCalculator, m_chooseWonderHandler, m_gameElements, m_randomGenerator, m_droppedCardListFactory, m_militaryBoardFactory));
            Assert.Throws<ArgumentNullException>(() => new GameContext(m_ageHandler, m_turnHandler, null, m_eventManager, m_costCalculator, m_chooseWonderHandler, m_gameElements, m_randomGenerator, m_droppedCardListFactory, m_militaryBoardFactory));
            Assert.Throws<ArgumentNullException>(() => new GameContext(m_ageHandler, m_turnHandler, m_playerActionReceiver, null, m_costCalculator, m_chooseWonderHandler, m_gameElements, m_randomGenerator, m_droppedCardListFactory, m_militaryBoardFactory));
            Assert.Throws<ArgumentNullException>(() => new GameContext(m_ageHandler, m_turnHandler, m_playerActionReceiver, m_eventManager, null, m_chooseWonderHandler, m_gameElements, m_randomGenerator, m_droppedCardListFactory, m_militaryBoardFactory));
            Assert.Throws<ArgumentNullException>(() => new GameContext(m_ageHandler, m_turnHandler, m_playerActionReceiver, m_eventManager, m_costCalculator, null, m_gameElements, m_randomGenerator, m_droppedCardListFactory, m_militaryBoardFactory));
            Assert.Throws<ArgumentNullException>(() => new GameContext(m_ageHandler, m_turnHandler, m_playerActionReceiver, m_eventManager, m_costCalculator, m_chooseWonderHandler, null, m_randomGenerator, m_droppedCardListFactory, m_militaryBoardFactory));
            Assert.Throws<ArgumentNullException>(() => new GameContext(m_ageHandler, m_turnHandler, m_playerActionReceiver, m_eventManager, m_costCalculator, m_chooseWonderHandler, m_gameElements, null, m_droppedCardListFactory, m_militaryBoardFactory));
            Assert.Throws<ArgumentNullException>(() => new GameContext(m_ageHandler, m_turnHandler, m_playerActionReceiver, m_eventManager, m_costCalculator, m_chooseWonderHandler, m_gameElements, m_randomGenerator, null, m_militaryBoardFactory));
            Assert.Throws<ArgumentNullException>(() => new GameContext(m_ageHandler, m_turnHandler, m_playerActionReceiver, m_eventManager, m_costCalculator, m_chooseWonderHandler, m_gameElements, m_randomGenerator, m_droppedCardListFactory, null));
        }

        [Test]
        public void When_Initialize_Called()
        {
            IMilitaryBoard militaryBoard = Substitute.For<IMilitaryBoard>();
            m_militaryBoardFactory.Create().Returns(militaryBoard);

            m_gameContext.Initialize([], [], []);

            m_militaryBoardFactory.Received(1).Create();
            m_droppedCardListFactory.Received(1).Create();
            _ = m_gameElements.Received(1).Cards;
            _ = m_gameElements.Received(1).Wonders;
            _ = m_gameElements.Received(1).Developments;
            m_chooseWonderHandler.Received(1).Initialize(Arg.Any<ICollection<Player>>(), Arg.Any<ICollection<Wonder>>());
            m_turnHandler.Received(1).Initialize(Arg.Any<ICollection<Player>>());
            m_eventManager.Received(1).ClearSubscriptions();
            m_ageHandler.Received(1).Initialize();
            militaryBoard.Received(1).Initialize(Arg.Any<ICollection<Player>>(), Arg.Any<ICollection<Development>>(), m_gameContext);
            Assert.That(m_gameContext.AgeHandler, Is.EqualTo(m_ageHandler));
            Assert.That(m_gameContext.TurnHandler, Is.EqualTo(m_turnHandler));
            Assert.That(m_gameContext.PlayerActionReceiver, Is.EqualTo(m_playerActionReceiver));
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
        private IPlayerActionReceiver m_playerActionReceiver;
        private IEventManager m_eventManager;
        private ICostCalculator m_costCalculator;
        private IChooseWonderHandler m_chooseWonderHandler;
        private IGameElements m_gameElements;
        private IRandomGenerator m_randomGenerator;
        private ICardListFactory m_droppedCardListFactory;
        private IMilitaryBoardFactory m_militaryBoardFactory;
        private GameContext m_gameContext;
    }
}
