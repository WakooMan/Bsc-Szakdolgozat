using GameLogic.Elements.GameCards;

namespace GameLogic_UnitTests.Elements.GameCards
{
    public class EmptyCardListFactoryTests
    {
        [SetUp]
        public void Setup()
        {
            m_cardList = new EmptyCardListFactory();
        }

        [Test]
        public void When_Create_Called()
        {
            ICardList cardList = m_cardList.Create();

            Assert.That(cardList, Is.Not.Null);
            Assert.That(cardList.GetType(), Is.EqualTo(typeof(CardList)));
        }

        private EmptyCardListFactory m_cardList;
    }
}
