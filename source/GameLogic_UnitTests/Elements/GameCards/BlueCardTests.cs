using GameLogic.Ages;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Goods;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.GameCards
{
    public class BlueCardTests
    {
        [SetUp]
        public void Setup()
        {
            m_victoryPoints = Substitute.For<VictoryPoints>();
            m_good = Substitute.For<Good>();
            m_blueCard = new BlueCard()
            {
                Name = "testName",
                PreviousBuilding = "testPrevious",
                GoodCost = new List<Good> { m_good },
                Age = AgesEnum.I,
                Point = m_victoryPoints
            };
        }

        [Test]
        public void When_Initialized_With_Default_Constructor()
        {
            m_blueCard = new BlueCard();

            Assert.That(m_blueCard, Is.Not.Null);
            Assert.That(m_blueCard.BuildingType, Is.EqualTo(typeof(BlueCard).Name));
            Assert.That(m_blueCard.Name, Is.EqualTo(string.Empty));
            Assert.That(m_blueCard.PreviousBuilding, Is.EqualTo(string.Empty));
            Assert.That(m_blueCard.GoodCost, Is.Not.Null);
            Assert.That(m_blueCard.GoodCost.Count, Is.EqualTo(0));
            Assert.That(m_blueCard.Age, Is.EqualTo(AgesEnum.None));

            Assert.That(m_blueCard.Point, Is.Not.Null);
            Assert.That(m_blueCard.Point.Points, Is.EqualTo(0));
        }

        [Test]
        public void When_Clone_Called()
        {
            BlueCard blueCard = m_blueCard.Clone();

            Assert.That(blueCard, Is.Not.Null);
            Assert.That(blueCard.Equals(m_blueCard), Is.False);
            Assert.That(blueCard.Name, Is.EqualTo(m_blueCard.Name));
            Assert.That(blueCard.PreviousBuilding, Is.EqualTo(m_blueCard.PreviousBuilding));
            Assert.That(blueCard.GoodCost, Is.Not.Null);
            Assert.That(blueCard.GoodCost.Count, Is.EqualTo(m_blueCard.GoodCost.Count));
            m_good.Received(1).Clone();
            Assert.That(blueCard.Age, Is.EqualTo(m_blueCard.Age));

            m_victoryPoints.Received(1).Clone();
        }

        private Good m_good;
        private VictoryPoints m_victoryPoints;
        private BlueCard m_blueCard;
    }
}
