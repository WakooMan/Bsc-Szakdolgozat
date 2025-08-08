using GameLogic;
using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.Elements.Disciplines;
using GameLogic.Events;
using GameLogic.GameStructures;
using GameLogic.Handlers;
using GameLogic.PlayerActions;
using NSubstitute;

namespace GameLogic_UnitTests.PlayerActions
{
    public class ChooseDisciplineActionTests
    {
        [SetUp]
        public void Setup()
        {
            m_disciplineTest = null;
            m_discipline = Substitute.For<Discipline>();
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
            m_chooseDisciplineAction = new ChooseDisciplineAction(m_discipline, (discipline) => m_disciplineTest = discipline);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new ChooseDisciplineAction(m_discipline, null));
            Assert.Throws<ArgumentNullException>(() => new ChooseDisciplineAction(null, (d) => { }));
        }

        [Test]
        public void When_CanPerform_Called()
        {
            bool result = m_chooseDisciplineAction.CanPerform(m_gameContext);

            Assert.That(result, Is.True);
        }

        [Test]
        public void When_DoPlayerAction_Called_Successful()
        {
            m_chooseDisciplineAction.DoPlayerAction(m_gameContext);

            Assert.That(m_disciplineTest, Is.Not.Null);
            Assert.That(m_disciplineTest, Is.EqualTo(m_discipline));
        }

        private ChooseDisciplineAction m_chooseDisciplineAction;
        private Discipline m_discipline;
        Discipline? m_disciplineTest;
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
