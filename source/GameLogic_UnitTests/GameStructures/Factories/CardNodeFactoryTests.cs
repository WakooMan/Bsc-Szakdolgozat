using GameLogic.Elements.GameCards;
using GameLogic.GameStructures;
using GameLogic.GameStructures.Factories;
using NSubstitute;

namespace GameLogic_UnitTests.GameStructures.Factories
{
    public class CardNodeFactoryTests
    {
        [SetUp]
        public void Setup()
        {
            m_cardNodeFactory = new CardNodeFactory();
        }

        [Test]
        public void When_Create_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => m_cardNodeFactory.Create(null));
        }

        [Test]
        public void When_Create_Called_With_Card()
        {
            Card card = Substitute.For<Card>();
            ICardNode cardNode = null;
            Assert.DoesNotThrow(() => cardNode = m_cardNodeFactory.Create(card));
            Assert.That(cardNode, Is.Not.Null);
        }

        private CardNodeFactory m_cardNodeFactory;
    }
}
