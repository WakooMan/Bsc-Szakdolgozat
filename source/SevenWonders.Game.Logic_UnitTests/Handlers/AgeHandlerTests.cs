using SevenWonders.Game.Logic.Ages;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.GameStructures.Factories;
using SevenWonders.Game.Logic.Handlers;
using NSubstitute;
using SevenWonders.Common;

namespace GameLogic_UnitTests.Handlers
{
    public class AgeHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            m_cardCompositionFactory = Substitute.For<ICardCompositionFactory>();
            m_gameElements = Substitute.For<IGameElements>();
            m_eventManager = Substitute.For<IEventManager>();
            m_randomGenerator = Substitute.For<IRandomGenerator>();
            m_cardList = Substitute.For<ICardList>();
            m_cardList.Cards.Returns(new List<Card>());
            m_gameElements.Cards.Returns(m_cardList);
            m_ageHandler = new AgeHandler(m_cardCompositionFactory, m_eventManager);
            m_ageHandler.Initialize(m_randomGenerator, m_gameElements.Cards);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new AgeHandler(null, m_eventManager));
            Assert.Throws<ArgumentNullException>(() => new AgeHandler(m_cardCompositionFactory, null));
        }

        [Test]
        public void When_Not_Initialized()
        {
            m_ageHandler = new AgeHandler(m_cardCompositionFactory, m_eventManager);
            Assert.Throws<InvalidOperationException>(() => { IAgeBase age = m_ageHandler.CurrentAge; });
            Assert.Throws<InvalidOperationException>(() => m_ageHandler.NextAge());
        }

        [Test]
        public void When_Initialized()
        {
            Assert.That(m_ageHandler.CurrentAge is FirstAge, Is.True);
            Assert.That(m_ageHandler.CurrentAge.Age == AgesEnum.I, Is.True);
        }

        [Test]
        public void When_NextAge_Called_Once()
        {
            bool result = m_ageHandler.NextAge();
            Assert.That(result, Is.True);
            Assert.That(m_ageHandler.CurrentAge is SecondAge, Is.True);
            Assert.That(m_ageHandler.CurrentAge.Age == AgesEnum.II, Is.True);
            m_eventManager.Received(1).Publish(Arg.Any<OnAgeEnded>());
        }

        [Test]
        public void When_NextAge_Called_Twice()
        {
            bool result = m_ageHandler.NextAge();
            bool result1 = m_ageHandler.NextAge();
            Assert.That(result, Is.True);
            Assert.That(result1, Is.True);
            Assert.That(m_ageHandler.CurrentAge is ThirdAge, Is.True);
            Assert.That(m_ageHandler.CurrentAge.Age == AgesEnum.III, Is.True);
            m_eventManager.Received(2).Publish(Arg.Any<OnAgeEnded>());
        }

        [Test]
        public void When_NextAge_Called_Three_times()
        {
            bool result = m_ageHandler.NextAge();
            bool result1 = m_ageHandler.NextAge();
            bool result2 = m_ageHandler.NextAge();
            Assert.That(result, Is.True);
            Assert.That(result1, Is.True);
            Assert.That(result2, Is.False);
            Assert.That(m_ageHandler.CurrentAge is ThirdAge, Is.True);
            Assert.That(m_ageHandler.CurrentAge.Age == AgesEnum.III, Is.True);
            m_eventManager.Received(2).Publish(Arg.Any<OnAgeEnded>());
        }

        private AgeHandler m_ageHandler;
        private ICardCompositionFactory m_cardCompositionFactory;
        private IGameElements m_gameElements;
        private ICardList m_cardList;
        private IEventManager m_eventManager;
        private IRandomGenerator m_randomGenerator;
    }
}
