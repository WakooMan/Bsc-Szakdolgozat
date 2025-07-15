using GameLogic.Ages;
using GameLogic.Elements.GameCards;
using GameLogic.GameStructures;
using GameLogic.GameStructures.Factories;
using GameLogic.Handlers.Factories;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Ages
{
    public class FirstAgeTests
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
            m_firstAge = new FirstAge(m_cardCompositionFactory, m_cardList);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new FirstAge(null, m_cardList));
            Assert.Throws<ArgumentNullException>(() => new FirstAge(m_cardCompositionFactory, null));
        }

        [Test]
        public void When_Initialized()
        {
            m_cardCompositionFactory.Received(1).Create(Arg.Any<string>(), Arg.Is<ICollection<ICard>>(coll => coll.SequenceEqual(new ICard[] { m_card1 })));
            Assert.That(m_firstAge.Composition == m_cardComposition, Is.True);
            Assert.That(m_firstAge.Age == AgesEnum.I);

        }

        private FirstAge m_firstAge;
        private ICardCompositionFactory m_cardCompositionFactory;
        private ICardComposition m_cardComposition;
        private ICardList m_cardList;
        private ICard m_card1;
        private ICard m_card2;
        private ICard m_card3;
    }
}
