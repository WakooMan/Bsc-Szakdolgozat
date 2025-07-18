using GameLogic.Ages;
using GameLogic.Elements.GameCards;
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
            m_cardList = Substitute.For<ICardList>();
            m_cardList.Cards.Returns(new List<Card>());
            m_ageHandler = new AgeHandler(m_cardCompositionFactory, m_cardList);
            m_NextAgeEventCalled = 0;
            m_ageHandler.HandleAgeChanged += (age) => m_NextAgeEventCalled++;
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new AgeHandler(null, m_cardList));
            Assert.Throws<ArgumentNullException>(() => new AgeHandler(m_cardCompositionFactory, null));
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
            Assert.That(m_NextAgeEventCalled == 1, Is.True);
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
            Assert.That(m_NextAgeEventCalled == 2, Is.True);
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
            Assert.That(m_NextAgeEventCalled == 2, Is.True);
        }

        private AgeHandler m_ageHandler;
        private ICardCompositionFactory m_cardCompositionFactory;
        private ICardList m_cardList;
        private int m_NextAgeEventCalled;
    }
}
