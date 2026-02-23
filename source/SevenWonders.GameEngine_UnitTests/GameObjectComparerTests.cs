using SevenWonders.GameEngine;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class GameObjectComparerTests
    {
        [SetUp]
        public void Setup()
        {
            m_gameObjectComparer = new GameObjectComparer();
        }

        [Test]
        public void When_Compare_Called_And_X_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => m_gameObjectComparer.Compare(null, new GameObject()));
        }

        [Test]
        public void When_Compare_Called_And_Y_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => m_gameObjectComparer.Compare(new GameObject(), null));
        }

        [TestCase(0, 1, -1)]
        [TestCase(1, 0, 1)]
        [TestCase(0, 0, 0)]
        public void When_Compare_Called_And_Returns(int xZIndex, int yZIndex, int expectedResult)
        {
            GameObject x = new GameObject()
            {
                ZIndex = xZIndex
            };

            GameObject y = new GameObject()
            {
                ZIndex = yZIndex
            };


            int result = m_gameObjectComparer.Compare(x, y);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private GameObjectComparer m_gameObjectComparer;
    }
}
