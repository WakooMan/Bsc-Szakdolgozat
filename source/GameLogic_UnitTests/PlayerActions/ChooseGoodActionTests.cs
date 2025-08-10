using GameLogic;
using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Factories;
using GameLogic.Elements.Goods.Resources;
using GameLogic.Events;
using GameLogic.GameStructures;
using GameLogic.Handlers;
using GameLogic.PlayerActions;
using NSubstitute;

namespace GameLogic_UnitTests.PlayerActions
{
    public class ChooseGoodActionTests
    {
        [SetUp]
        public void Setup()
        {
            m_good = null;
            m_clay = new Clay();
            m_goodFactory = Substitute.For<GoodFactory>();
            m_goodFactory.CreateGood().Returns(m_clay);
            m_costCalculator = Substitute.For<ICostCalculator>();
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_gameContext = Substitute.For<IGameContext>();
            m_cardComposition = Substitute.For<ICardComposition>();
            m_eventManager = Substitute.For<IEventManager>();
            m_age = Substitute.For<IAgeBase>();
            m_ageHandler = Substitute.For<IAgeHandler>();
            m_current = new Player() { Name = "Current" };
            m_opponent = new Player() { Name = "Opponent" };
            m_turnHandler.CurrentPlayer.Returns(m_current);
            m_turnHandler.OpponentPlayer.Returns(m_opponent);
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_gameContext.CostCalculator.Returns(m_costCalculator);
            m_gameContext.AgeHandler.Returns(m_ageHandler);
            m_gameContext.EventManager.Returns(m_eventManager);
            m_ageHandler.CurrentAge.Returns(m_age);
            m_age.Composition.Returns(m_cardComposition);
            m_chooseGoodAction = new ChooseGoodAction(m_goodFactory, (good) => m_good = good);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new ChooseGoodAction(m_goodFactory, null));
            Assert.Throws<ArgumentNullException>(() => new ChooseGoodAction(null, (d) => { }));
        }

        [Test]
        public void When_CanPerform_Called()
        {
            bool result = m_chooseGoodAction.CanPerform(m_gameContext);

            Assert.That(result, Is.True);
        }

        [Test]
        public void When_DoPlayerAction_Called_Successful()
        {
            m_chooseGoodAction.DoPlayerAction(m_gameContext);

            m_goodFactory.Received(1).CreateGood();
            Assert.That(m_good, Is.Not.Null);
            Assert.That(m_good, Is.EqualTo(m_clay));
        }

        private ChooseGoodAction m_chooseGoodAction;
        private GoodFactory m_goodFactory;
        private Good? m_good;
        private Clay m_clay;
        private Player m_current;
        private Player m_opponent;
        private ICostCalculator m_costCalculator;
        private ITurnHandler m_turnHandler;
        private IGameContext m_gameContext;
        private IAgeHandler m_ageHandler;
        private IAgeBase m_age;
        private ICardComposition m_cardComposition;
        private IEventManager m_eventManager;
    }
}
