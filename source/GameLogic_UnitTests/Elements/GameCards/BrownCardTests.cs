using GameLogic.Ages;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Resources;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.GameCards
{
    public class BrownCardTests
    {
        [SetUp]
        public void Setup()
        {
            m_good = Substitute.For<Good>();
            m_gameResource = Substitute.For<GameResource>();
            m_brownCard = new BrownCard()
            {
                Name = "testName",
                PreviousBuilding = "testPrevious",
                GoodCost = new List<Good> { m_good },
                Age = AgesEnum.I,
                ProducedResources = new List<GameResource>() { m_gameResource }
            };
        }

        [Test]
        public void When_Initialized_With_Default_Constructor()
        {
            m_brownCard = new BrownCard();

            Assert.That(m_brownCard, Is.Not.Null);
            Assert.That(m_brownCard.BuildingType, Is.EqualTo(typeof(BrownCard).Name));
            Assert.That(m_brownCard.Name, Is.EqualTo(string.Empty));
            Assert.That(m_brownCard.PreviousBuilding, Is.EqualTo(string.Empty));
            Assert.That(m_brownCard.GoodCost, Is.Not.Null);
            Assert.That(m_brownCard.GoodCost.Count, Is.EqualTo(0));
            Assert.That(m_brownCard.Age, Is.EqualTo(AgesEnum.None));

            Assert.That(m_brownCard.ProducedResources, Is.Not.Null);
            Assert.That(m_brownCard.ProducedResources.Count, Is.EqualTo(0));
        }

        [Test]
        public void When_Clone_Called()
        {
            BrownCard brownCard = m_brownCard.Clone();

            Assert.That(brownCard, Is.Not.Null);
            Assert.That(brownCard.Equals(m_brownCard), Is.False);
            Assert.That(brownCard.Name, Is.EqualTo(m_brownCard.Name));
            Assert.That(brownCard.PreviousBuilding, Is.EqualTo(m_brownCard.PreviousBuilding));
            Assert.That(brownCard.GoodCost, Is.Not.Null);
            Assert.That(brownCard.GoodCost.Count, Is.EqualTo(m_brownCard.GoodCost.Count));
            m_good.Received(1).Clone();
            Assert.That(brownCard.Age, Is.EqualTo(m_brownCard.Age));

            Assert.That(brownCard.ProducedResources, Is.Not.Null);
            Assert.That(brownCard.ProducedResources.Count, Is.EqualTo(m_brownCard.ProducedResources.Count));
            m_gameResource.Received(1).Clone();
        }

        private GameResource m_gameResource;
        private Good m_good;
        private BrownCard m_brownCard;
    }
}
