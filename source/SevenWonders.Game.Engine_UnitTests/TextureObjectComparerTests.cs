using SevenWonders.Game.Engine;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class TextureObjectComparerTests
    {
        [SetUp]
        public void Setup()
        {
            m_sceneObjectComparer = new SceneObjectComparer();
        }

        [Test]
        public void When_Compare_Called_And_X_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => m_sceneObjectComparer.Compare(null, new TextureObject()));
        }

        [Test]
        public void When_Compare_Called_And_Y_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => m_sceneObjectComparer.Compare(new TextureObject(), null));
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


            int result = m_sceneObjectComparer.Compare(x, y);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private SceneObjectComparer m_sceneObjectComparer;
    }
}
