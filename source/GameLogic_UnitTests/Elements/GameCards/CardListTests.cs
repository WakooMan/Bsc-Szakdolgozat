using GameLogic.Elements.GameCards;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.GameCards
{
    public class CardListTests
    {
        [SetUp]
        public void Setup()
        {
            m_card = Substitute.For<Card>();
            m_cardList = new CardList()
            {
                Cards = [m_card]
            };
        }

        [Test]
        public void When_Initialized_With_Default_Construtor()
        {
            m_cardList = new CardList();

            Assert.That(m_cardList, Is.Not.Null);
            Assert.That(m_cardList.Cards, Is.Not.Null);
            Assert.That(m_cardList.Cards.Count, Is.EqualTo(0));
        }

        [Test]
        public void When_Clone_Called()
        {
            ICardList cardList = m_cardList.Clone();

            Assert.That(cardList, Is.Not.Null);
            Assert.That(cardList.Cards, Is.Not.Null);
            Assert.That(cardList.Cards.Count, Is.EqualTo(m_cardList.Cards.Count));
            m_card.Received(1).Clone();
        }

        private Card m_card;
        private CardList m_cardList;
    }
}
