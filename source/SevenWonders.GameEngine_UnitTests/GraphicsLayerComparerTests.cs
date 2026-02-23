using SevenWonders.GameEngine;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class GraphicsLayerComparerTests
    {
        [SetUp]
        public void Setup()
        {
            m_layerComparer = new GraphicsLayerComparer();
        }

        [Test]
        public void When_Compare_Called_And_X_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => m_layerComparer.Compare(null, new GraphicsLayer()));
        }

        [Test]
        public void When_Compare_Called_And_Y_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => m_layerComparer.Compare(new GraphicsLayer(), null));
        }

        [TestCase(0, 1, -1)]
        [TestCase(1, 0, 1)]
        [TestCase(0, 0, 0)]
        public void When_Compare_Called_And_Returns(int xZIndex, int yZIndex, int expectedResult)
        {
            GraphicsLayer x = new GraphicsLayer()
            {
                ZIndex = xZIndex
            };

            GraphicsLayer y = new GraphicsLayer()
            {
                ZIndex = yZIndex
            };


            int result = m_layerComparer.Compare(x, y);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private GraphicsLayerComparer m_layerComparer;
    }
}
