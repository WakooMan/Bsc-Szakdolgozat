using GameLogic;
using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Factories;
using GameLogic.Elements.Goods.Resources;
using GameLogic.Elements.Wonders;
using GameLogic.Events;
using GameLogic.GameStructures;
using GameLogic.Handlers;
using GameLogic.PlayerActions;
using NSubstitute;

namespace GameLogic_UnitTests.PlayerActions
{
    public class ChooseWonderActionTests
    {
        [SetUp]
        public void Setup()
        {
            m_wonder = new Wonder();
            m_wonders = new List<Wonder>();
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
            m_chooseGoodAction = new ChooseWonderAction(m_wonder, m_wonders, () => m_current);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new ChooseWonderAction(null, [], () => m_current));
            Assert.Throws<ArgumentNullException>(() => new ChooseWonderAction(m_wonder, null , () => m_current));
            Assert.Throws<ArgumentNullException>(() => new ChooseWonderAction(m_wonder, [], null));
        }

        [Test]
        public void When_CanPerform_Called_And_Wonder_List_Does_Not_Contain_Wonder()
        {
            bool result = m_chooseGoodAction.CanPerform(m_gameContext);

            Assert.That(result, Is.False);
        }

        [Test]
        public void When_CanPerform_Called_Successfull()
        {
            m_wonders.Add(m_wonder);

            bool result = m_chooseGoodAction.CanPerform(m_gameContext);

            Assert.That(result, Is.True);
        }

        [Test]
        public void When_DoPlayerAction_Called_And_Wonder_List_Does_Not_Contain_Wonder()
        {
            Assert.Throws<InvalidOperationException>(() => m_chooseGoodAction.DoPlayerAction(m_gameContext));
        }

        [Test]
        public void When_DoPlayerAction_Called_Successful()
        {
            m_wonders.Add(m_wonder);

            m_chooseGoodAction.DoPlayerAction(m_gameContext);

            Assert.That(m_current.Wonders.Contains(m_wonder), Is.True);
            Assert.That(m_wonders.Contains(m_wonder), Is.False);
        }

        private List<Wonder> m_wonders;
        private Wonder m_wonder;
        private ChooseWonderAction m_chooseGoodAction;
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
