using GameLogic.Elements.GameCards;
using GameLogic.GameStructures;
using GameLogic.GameStructures.Factories;
using GameLogic.Handlers;
using GameLogic.Handlers.Factories;
using NSubstitute;

namespace GameLogic_UnitTests.GameStructures.Factories
{
    public class CardCompositionFactoryTests
    {
        [SetUp]
        public void Setup()
        {
            m_cardCompositionFileHandlerFactory = Substitute.For<ICardCompositionFileHandlerFactory>();
            m_cardNodeFactory = Substitute.For<ICardNodeFactory>();
            m_cardCompositionFactory = new CardCompositionFactory(m_cardCompositionFileHandlerFactory, m_cardNodeFactory);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new CardCompositionFactory(null, m_cardNodeFactory));
            Assert.Throws<ArgumentNullException>(() => new CardCompositionFactory(m_cardCompositionFileHandlerFactory, null));
        }

        [Test]
        public void When_Create_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => m_cardCompositionFactory.Create(null, new List<Card>()));
            Assert.Throws<ArgumentNullException>(() => m_cardCompositionFactory.Create("", new List<Card>()));
            Assert.Throws<ArgumentNullException>(() => m_cardCompositionFactory.Create("something", null));
        }

        [Test]
        public void When_Create_Called_With_Arguments()
        {
            List<Card> cards = new List<Card>();
            for (int i = 0; i < 20; i++)
            {
                cards.Add(Substitute.For<Card>());
            }
            ICardComposition cardComposition = null;
            m_cardCompositionFileHandlerFactory.CreateCardCompositionFileHandler(Arg.Any<string>()).Returns(Substitute.For<ICardCompositionFileHandler>());
            Assert.DoesNotThrow(() => cardComposition = m_cardCompositionFactory.Create("something", cards));
            Assert.That(cardComposition, Is.Not.Null);
        }

        private CardCompositionFactory m_cardCompositionFactory;
        private ICardCompositionFileHandlerFactory m_cardCompositionFileHandlerFactory;
        private ICardNodeFactory m_cardNodeFactory;
    }
}
