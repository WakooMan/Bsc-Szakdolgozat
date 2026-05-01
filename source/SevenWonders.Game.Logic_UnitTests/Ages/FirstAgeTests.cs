using SevenWonders.Game.Logic.Ages;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.GameStructures;
using SevenWonders.Game.Logic.GameStructures.Factories;
using NSubstitute;
using SevenWonders.Common;

namespace GameLogic_UnitTests.Ages
{
    public class FirstAgeTests
    {
        [SetUp]
        public void Setup()
        {
            m_eventManager = Substitute.For<IEventManager>();
            m_cardCompositionFactory = Substitute.For<ICardCompositionFactory>();
            m_cardComposition = Substitute.For<ICardComposition>();
            m_cardCompositionFactory.Create(Arg.Any<string>(), Arg.Any<ICollection<Card>>()).Returns(m_cardComposition);
            m_cardList = Substitute.For<ICardList>();
            m_randomGenerator = Substitute.For<IRandomGenerator>();
            m_card1 = new BrownCard() { Age = AgesEnum.I };
            m_card2 = new BrownCard() { Age = AgesEnum.II };
            m_card3 = new BrownCard() { Age = AgesEnum.III };
            m_cardList.Cards.Returns(new List<Card>() { m_card1, m_card2, m_card3 });
            m_randomGenerator.ReceiveRandomElements(Arg.Any<ICollection<Card>>(), 20).Returns(callInfo => callInfo.ArgAt<ICollection<Card>>(0));
            m_firstAge = new FirstAge(m_eventManager, m_cardCompositionFactory, m_cardList, m_randomGenerator);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new FirstAge(null, m_cardCompositionFactory, m_cardList, m_randomGenerator));
            Assert.Throws<ArgumentNullException>(() => new FirstAge(m_eventManager, null, m_cardList, m_randomGenerator));
            Assert.Throws<ArgumentNullException>(() => new FirstAge(m_eventManager, m_cardCompositionFactory, null, m_randomGenerator));
            Assert.Throws<ArgumentNullException>(() => new FirstAge(m_eventManager, m_cardCompositionFactory, m_cardList, null));
        }

        [Test]
        public void When_Initialized()
        {
            m_cardCompositionFactory.Received(1).Create(Arg.Any<string>(), Arg.Is<ICollection<Card>>(coll => coll.SequenceEqual(new Card[] { m_card1 })));
            Assert.That(m_firstAge.Composition == m_cardComposition, Is.True);
            Assert.That(m_firstAge.Age == AgesEnum.I);

        }

        private IEventManager m_eventManager;
        private IRandomGenerator m_randomGenerator;
        private FirstAge m_firstAge;
        private ICardCompositionFactory m_cardCompositionFactory;
        private ICardComposition m_cardComposition;
        private ICardList m_cardList;
        private Card m_card1;
        private Card m_card2;
        private Card m_card3;
    }
}
