using SevenWonders.GameEngine;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class TextureObjectComparerTests
    {
        [SetUp]
        public void Setup()
        {
            m_textureComparer = new TextureObjectComparer();
        }

        [Test]
        public void When_Compare_Called_And_X_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => m_textureComparer.Compare(null, new TextureObject()));
        }

        [Test]
        public void When_Compare_Called_And_Y_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => m_textureComparer.Compare(new TextureObject(), null));
        }

        [TestCase(0, 1, -1)]
        [TestCase(1, 0, 1)]
        [TestCase(0, 0, 0)]
        public void When_Compare_Called_And_Returns(int xZIndex, int yZIndex, int expectedResult)
        {
            TextureObject x = new TextureObject()
            {
                ZIndex = xZIndex
            };

            TextureObject y = new TextureObject()
            {
                ZIndex = yZIndex
            };


            int result = m_textureComparer.Compare(x, y);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private TextureObjectComparer m_textureComparer;
    }
}
