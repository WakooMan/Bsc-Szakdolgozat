using SevenWonders.Game.Logic.Handlers;

namespace GameLogic_UnitTests.Handlers
{
    public class CardCompositionFileHandlerTests
    {
        [Test]
        public void When_Constructor_Called_With_Null_Or_Empty_String()
        {
            Assert.Throws<ArgumentNullException>(() => new CardCompositionFileHandler(null));
            Assert.Throws<ArgumentNullException>(() => new CardCompositionFileHandler(""));
        }

        [Test]
        public void When_Constructor_Called()
        {
            string compositionFile = Path.Combine(Path.GetTempPath(), "TestData.csv");
            CardCompositionFileHandler? cardCompositionFileHandler = null;
            Assert.DoesNotThrow(() => cardCompositionFileHandler = new CardCompositionFileHandler(compositionFile));
            Assert.That(cardCompositionFileHandler, Is.Not.Null);
        }
    }
}
