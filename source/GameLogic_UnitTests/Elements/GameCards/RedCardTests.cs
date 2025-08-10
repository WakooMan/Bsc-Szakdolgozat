using GameLogic.Ages;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Goods;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.GameCards
{
    public class RedCardTests
    {
        [SetUp]
        public void Setup()
        {
            m_good = Substitute.For<Good>();
            m_strength = Substitute.For<Strength>();
            m_redCard = new RedCard()
            {
                Name = "testName",
                PreviousBuilding = "testPrevious",
                GoodCost = new List<Good> { m_good },
                Age = AgesEnum.I,
                Strength = m_strength
            };
        }

        [Test]
        public void When_Initialized_With_Default_Constructor()
        {
            m_redCard = new RedCard();

            Assert.That(m_redCard, Is.Not.Null);
            Assert.That(m_redCard.BuildingType, Is.EqualTo(typeof(RedCard).Name));
            Assert.That(m_redCard.Name, Is.EqualTo(string.Empty));
            Assert.That(m_redCard.PreviousBuilding, Is.EqualTo(string.Empty));
            Assert.That(m_redCard.GoodCost, Is.Not.Null);
            Assert.That(m_redCard.GoodCost.Count, Is.EqualTo(0));
            Assert.That(m_redCard.Age, Is.EqualTo(AgesEnum.None));

            Assert.That(m_redCard.Strength, Is.Not.Null);
            Assert.That(m_redCard.Strength.Points, Is.EqualTo(0));
        }

        [Test]
        public void When_Clone_Called()
        {
            RedCard redCard = m_redCard.Clone();

            Assert.That(redCard, Is.Not.Null);
            Assert.That(redCard.Equals(m_redCard), Is.False);
            Assert.That(redCard.Name, Is.EqualTo(m_redCard.Name));
            Assert.That(redCard.PreviousBuilding, Is.EqualTo(m_redCard.PreviousBuilding));
            Assert.That(redCard.GoodCost, Is.Not.Null);
            Assert.That(redCard.GoodCost.Count, Is.EqualTo(m_redCard.GoodCost.Count));
            m_good.Received(1).Clone();
            Assert.That(redCard.Age, Is.EqualTo(m_redCard.Age));

            m_strength.Received(1).Clone();
        }

        private Strength m_strength;
        private Good m_good;
        private RedCard m_redCard;
    }
}
