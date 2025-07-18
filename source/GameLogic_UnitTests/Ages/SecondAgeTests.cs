using GameLogic.Ages;
using GameLogic.Elements.GameCards;
using GameLogic.GameStructures;
using GameLogic.GameStructures.Factories;
using NSubstitute;

namespace GameLogic_UnitTests.Ages
{
    public class SecondAgeTests
    {
        [SetUp]
        public void Setup()
        {
            m_cardCompositionFactory = Substitute.For<ICardCompositionFactory>();
            m_cardComposition = Substitute.For<ICardComposition>();
            m_cardCompositionFactory.Create(Arg.Any<string>(), Arg.Any<ICollection<Card>>()).Returns(m_cardComposition);
            m_cardList = Substitute.For<ICardList>();
            m_card1 = Substitute.For<Card>();
            m_card1.Age.Returns(AgesEnum.I);
            m_card2 = Substitute.For<Card>();
            m_card2.Age.Returns(AgesEnum.II);
            m_card3 = Substitute.For<Card>();
            m_card3.Age.Returns(AgesEnum.III);
            m_cardList.Cards.Returns(new List<Card>() { m_card1, m_card2, m_card3 });
            m_secondAge = new SecondAge(m_cardCompositionFactory, m_cardList);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new SecondAge(null, m_cardList));
            Assert.Throws<ArgumentNullException>(() => new SecondAge(m_cardCompositionFactory, null));
        }

        [Test]
        public void When_Initialized()
        {
            m_cardCompositionFactory.Received(1).Create(Arg.Any<string>(), Arg.Is<ICollection<Card>>(coll => coll.SequenceEqual(new Card[] { m_card2 })));
            Assert.That(m_secondAge.Composition == m_cardComposition, Is.True);
            Assert.That(m_secondAge.Age == AgesEnum.II);

        }

        private SecondAge m_secondAge;
        private ICardCompositionFactory m_cardCompositionFactory;
        private ICardComposition m_cardComposition;
        private ICardList m_cardList;
        private Card m_card1;
        private Card m_card2;
        private Card m_card3;
    }
}
