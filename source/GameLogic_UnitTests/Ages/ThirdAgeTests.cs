using GameLogic.Ages;
using GameLogic.Elements.GameCards;
using GameLogic.GameStructures;
using GameLogic.GameStructures.Factories;
using NSubstitute;

namespace GameLogic_UnitTests.Ages
{
    public class ThirdAgeTests
    {
        [SetUp]
        public void Setup()
        {
            m_cardCompositionFactory = Substitute.For<ICardCompositionFactory>();
            m_cardComposition = Substitute.For<ICardComposition>();
            m_cardCompositionFactory.Create(Arg.Any<string>(), Arg.Any<ICollection<ICard>>()).Returns(m_cardComposition);
            m_cardList = Substitute.For<ICardList>();
            m_card1 = Substitute.For<ICard>();
            m_card1.Age.Returns(AgesEnum.I);
            m_card2 = Substitute.For<ICard>();
            m_card2.Age.Returns(AgesEnum.II);
            m_card3 = Substitute.For<ICard>();
            m_card3.Age.Returns(AgesEnum.III);
            m_cardList.Cards.Returns(new List<ICard>(new ICard[] { m_card1, m_card2, m_card3 }));
            m_thirdAge = new ThirdAge(m_cardCompositionFactory, m_cardList);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new ThirdAge(null, m_cardList));
            Assert.Throws<ArgumentNullException>(() => new ThirdAge(m_cardCompositionFactory, null));
        }

        [Test]
        public void When_Initialized()
        {
            m_cardCompositionFactory.Received(1).Create(Arg.Any<string>(), Arg.Is<ICollection<ICard>>(coll => coll.SequenceEqual(new ICard[] { m_card3 })));
            Assert.That(m_thirdAge.Composition == m_cardComposition, Is.True);
            Assert.That(m_thirdAge.Age == AgesEnum.III);

        }

        private ThirdAge m_thirdAge;
        private ICardCompositionFactory m_cardCompositionFactory;
        private ICardComposition m_cardComposition;
        private ICardList m_cardList;
        private ICard m_card1;
        private ICard m_card2;
        private ICard m_card3;
    }
}
