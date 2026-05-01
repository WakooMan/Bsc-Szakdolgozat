using NSubstitute;
using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Disciplines;
using SevenWonders.Game.Logic.Elements.Military;
using SevenWonders.Game.Logic.Elements.Modifiers;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Handlers;
using SevenWonders.Game.Logic.Interfaces;

namespace GameLogic_UnitTests.Elements.Disciplines
{
    [TestFixture]
    public class DisciplineTests
    {
        private IGameContext m_gameContext;
        private IEventManager m_eventManager;
        private IPlayerActionHandler m_playerActionHandler;
        private IMilitaryBoard m_militaryBoard;
        private Player m_owner;
        private Player m_opponent;
        private IPlayerActionReceiver m_receiver;

        [SetUp]
        public void Setup()
        {
            m_gameContext = Substitute.For<IGameContext>();
            m_eventManager = Substitute.For<IEventManager>();
            m_playerActionHandler = Substitute.For<IPlayerActionHandler>();
            m_militaryBoard = Substitute.For<IMilitaryBoard>();
            m_gameContext.EventManager.Returns(m_eventManager);
            m_gameContext.PlayerActionHandler.Returns(m_playerActionHandler);
            m_gameContext.MilitaryBoard.Returns(m_militaryBoard);
            m_militaryBoard.Developments.Returns(new List<Development>());

            m_receiver = Substitute.For<IPlayerActionReceiver>();
            m_owner = new Player(m_receiver, "Owner", 1, 10);
            m_opponent = new Player(m_receiver, "Opponent", 2, 10);
        }

        [Test]
        public void When_OnCalculatePlayerProperties_Called_ShouldAddDiscipline()
        {
            var discipline = new Building();
            var properties = new PlayerProperties(m_owner, m_opponent);

            discipline.OnCalculatePlayerProperties(properties);

            Assert.That(properties.Disciplines.ContainsKey(typeof(Building)), Is.True);
            Assert.That(properties.Disciplines[typeof(Building)], Is.EqualTo(1));
        }

        [Test]
        public void When_OnCalculatePlayerProperties_Called_Twice_ShouldIncrementCount()
        {
            var discipline = new Building();
            var properties = new PlayerProperties(m_owner, m_opponent);

            discipline.OnCalculatePlayerProperties(properties);
            discipline.OnCalculatePlayerProperties(properties);

            Assert.That(properties.Disciplines[typeof(Building)], Is.EqualTo(2));
        }

        [Test]
        public void When_Apply_Called_WithLessThanThreeDisciplines_NoDevelopments_ShouldNotPublish()
        {
            var discipline = new Building();
            discipline.Apply(m_gameContext, m_owner, m_opponent);

            m_eventManager.DidNotReceiveWithAnyArgs().Publish(Arg.Any<SevenWonders.Game.Logic.Events.GameEvents.GameEvent>());
        }

        [Test]
        public void When_Apply_Called_WithThreeSameDisciplines_AndDevelopmentsAvailable_ShouldOfferChoice()
        {
            var properties = new PlayerProperties(m_owner, m_opponent);
            var building = new Building();
            building.OnCalculatePlayerProperties(properties);
            building.OnCalculatePlayerProperties(properties);
            Assert.That(properties.Disciplines[typeof(Building)], Is.EqualTo(2));

            m_militaryBoard.Developments.Returns(new List<Development>());
            building.Apply(m_gameContext, m_owner, m_opponent);

            m_playerActionHandler.DidNotReceiveWithAnyArgs().HandlePlayerActions(default!, default!, default!);
        }

        [Test]
        public void When_Clone_Called_Building_ShouldReturnNewInstance()
        {
            var discipline = new Building();
            var clone = discipline.Clone();

            Assert.That(clone, Is.TypeOf<Building>());
            Assert.That(ReferenceEquals(clone, discipline), Is.False);
        }

        [Test]
        public void When_Clone_Called_DefaultDiscipline_ShouldReturnNewInstance()
        {
            var discipline = new DefaultDiscipline();
            var clone = discipline.Clone();

            Assert.That(clone, Is.TypeOf<DefaultDiscipline>());
        }

        [Test]
        public void When_Clone_Called_Geography_ShouldReturnNewInstance()
        {
            var discipline = new Geography();
            var clone = discipline.Clone();

            Assert.That(clone, Is.TypeOf<Geography>());
        }

        [Test]
        public void When_Clone_Called_Healing_ShouldReturnNewInstance()
        {
            var discipline = new Healing();
            var clone = discipline.Clone();

            Assert.That(clone, Is.TypeOf<Healing>());
        }

        [Test]
        public void When_Clone_Called_Mechanics_ShouldReturnNewInstance()
        {
            var discipline = new Mechanics();
            var clone = discipline.Clone();

            Assert.That(clone, Is.TypeOf<Mechanics>());
        }

        [Test]
        public void When_Clone_Called_Physics_ShouldReturnNewInstance()
        {
            var discipline = new Physics();
            var clone = discipline.Clone();

            Assert.That(clone, Is.TypeOf<Physics>());
        }

        [Test]
        public void When_Clone_Called_Trading_ShouldReturnNewInstance()
        {
            var discipline = new Trading();
            var clone = discipline.Clone();

            Assert.That(clone, Is.TypeOf<Trading>());
        }

        [Test]
        public void When_Clone_Called_Writing_ShouldReturnNewInstance()
        {
            var discipline = new Writing();
            var clone = discipline.Clone();

            Assert.That(clone, Is.TypeOf<Writing>());
        }
    }
}
