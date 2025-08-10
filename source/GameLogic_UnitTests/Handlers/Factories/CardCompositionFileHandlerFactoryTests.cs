using GameLogic.Handlers;
using GameLogic.Handlers.Factories;

namespace GameLogic_UnitTests.Handlers.Factories
{
    public class CardCompositionFileHandlerFactoryTests
    {
        [SetUp]
        public void Setup()
        {
            m_cardCompositionFileHandlerFactory = new CardCompositionFileHandlerFactory();
        }

        [Test]
        public void When_CreateCardCompositionFileHandler_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => m_cardCompositionFileHandlerFactory.CreateCardCompositionFileHandler(null));
        }

        [Test]
        public void When_CreateCardCompositionFileHandler_Called_With_Arguments()
        {
            ICardCompositionFileHandler cardCompositionFileHandler = null;
            Assert.DoesNotThrow(() => cardCompositionFileHandler = m_cardCompositionFileHandlerFactory.CreateCardCompositionFileHandler("something"));
            Assert.That(cardCompositionFileHandler, Is.Not.Null);
        }

        private CardCompositionFileHandlerFactory m_cardCompositionFileHandlerFactory;
    }
}
