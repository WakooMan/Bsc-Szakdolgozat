using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.GameStructures.Factories;
using GameLogic.Handlers;
using NSubstitute;

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
            m_cardList = Substitute.For<ICardList>();
            m_cardList.Cards.Returns(new List<Card>());
            m_gameElements.Cards.Returns(m_cardList);
            m_ageHandler = new AgeHandler(m_cardCompositionFactory, m_gameElements, m_eventManager);
            m_ageHandler.Initialize();
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new AgeHandler(null, m_gameElements, m_eventManager));
            Assert.Throws<ArgumentNullException>(() => new AgeHandler(m_cardCompositionFactory, null, m_eventManager));
            Assert.Throws<ArgumentNullException>(() => new AgeHandler(m_cardCompositionFactory, m_gameElements, null));
        }

        [Test]
        public void When_Not_Initialized()
        {
            m_ageHandler = new AgeHandler(m_cardCompositionFactory, m_gameElements, m_eventManager);
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
    }
}
