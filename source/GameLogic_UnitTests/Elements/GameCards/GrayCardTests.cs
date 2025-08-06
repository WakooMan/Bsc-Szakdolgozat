using GameLogic.Ages;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Products;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.GameCards
{
    public class GrayCardTests
    {
        [SetUp]
        public void Setup()
        {
            m_good = Substitute.For<Good>();
            m_product = Substitute.For<Product>();
            m_grayCard = new GrayCard()
            {
                Name = "testName",
                PreviousBuilding = "testPrevious",
                GoodCost = new List<Good> { m_good },
                Age = AgesEnum.I,
                CreatedProducts = new List<Product>() { m_product }
            };
        }

        [Test]
        public void When_Initialized_With_Default_Constructor()
        {
            m_grayCard = new GrayCard();

            Assert.That(m_grayCard, Is.Not.Null);
            Assert.That(m_grayCard.BuildingType, Is.EqualTo(typeof(GrayCard).Name));
            Assert.That(m_grayCard.Name, Is.EqualTo(string.Empty));
            Assert.That(m_grayCard.PreviousBuilding, Is.EqualTo(string.Empty));
            Assert.That(m_grayCard.GoodCost, Is.Not.Null);
            Assert.That(m_grayCard.GoodCost.Count, Is.EqualTo(0));
            Assert.That(m_grayCard.Age, Is.EqualTo(AgesEnum.None));

            Assert.That(m_grayCard.CreatedProducts, Is.Not.Null);
            Assert.That(m_grayCard.CreatedProducts.Count, Is.EqualTo(0));
        }

        [Test]
        public void When_Clone_Called()
        {
            GrayCard grayCard = m_grayCard.Clone();

            Assert.That(grayCard, Is.Not.Null);
            Assert.That(grayCard.Equals(m_grayCard), Is.False);
            Assert.That(grayCard.Name, Is.EqualTo(m_grayCard.Name));
            Assert.That(grayCard.PreviousBuilding, Is.EqualTo(m_grayCard.PreviousBuilding));
            Assert.That(grayCard.GoodCost, Is.Not.Null);
            Assert.That(grayCard.GoodCost.Count, Is.EqualTo(m_grayCard.GoodCost.Count));
            m_good.Received(1).Clone();
            Assert.That(grayCard.Age, Is.EqualTo(m_grayCard.Age));

            Assert.That(grayCard.CreatedProducts, Is.Not.Null);
            Assert.That(grayCard.CreatedProducts.Count, Is.EqualTo(m_grayCard.CreatedProducts.Count));
            m_product.Received(1).Clone();
        }

        private Product m_product;
        private Good m_good;
        private GrayCard m_grayCard;
    }
}
