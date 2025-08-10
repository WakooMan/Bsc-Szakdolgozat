using GameLogic.Ages;
using GameLogic.Elements.Disciplines;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Goods;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.GameCards
{
    public class GreenCardTests
    {
        [SetUp]
        public void Setup()
        {
            m_good = Substitute.For<Good>();
            m_discipline = Substitute.For<Discipline>();
            m_victoryPoint = Substitute.For<VictoryPoints>();
            m_greenCard = new GreenCard()
            {
                Name = "testName",
                PreviousBuilding = "testPrevious",
                GoodCost = new List<Good> { m_good },
                Age = AgesEnum.I,
                Point = m_victoryPoint,
                Discipline = m_discipline
            };
        }

        [Test]
        public void When_Initialized_With_Default_Constructor()
        {
            m_greenCard = new GreenCard();

            Assert.That(m_greenCard, Is.Not.Null);
            Assert.That(m_greenCard.BuildingType, Is.EqualTo(typeof(GreenCard).Name));
            Assert.That(m_greenCard.Name, Is.EqualTo(string.Empty));
            Assert.That(m_greenCard.PreviousBuilding, Is.EqualTo(string.Empty));
            Assert.That(m_greenCard.GoodCost, Is.Not.Null);
            Assert.That(m_greenCard.GoodCost.Count, Is.EqualTo(0));
            Assert.That(m_greenCard.Age, Is.EqualTo(AgesEnum.None));

            Assert.That(m_greenCard.Discipline, Is.Not.Null);
            Assert.That(m_greenCard.Discipline.GetType(), Is.EqualTo(typeof(DefaultDiscipline)));
            Assert.That(m_greenCard.Point, Is.Not.Null);
            Assert.That(m_greenCard.Point.Points, Is.EqualTo(0));
        }

        [Test]
        public void When_Clone_Called()
        {
            GreenCard greenCard = m_greenCard.Clone();

            Assert.That(greenCard, Is.Not.Null);
            Assert.That(greenCard.Equals(m_greenCard), Is.False);
            Assert.That(greenCard.Name, Is.EqualTo(m_greenCard.Name));
            Assert.That(greenCard.PreviousBuilding, Is.EqualTo(m_greenCard.PreviousBuilding));
            Assert.That(greenCard.GoodCost, Is.Not.Null);
            Assert.That(greenCard.GoodCost.Count, Is.EqualTo(m_greenCard.GoodCost.Count));
            m_good.Received(1).Clone();
            Assert.That(greenCard.Age, Is.EqualTo(m_greenCard.Age));

            m_discipline.Received(1).Clone();
            m_victoryPoint.Received(1).Clone();
        }

        private Discipline m_discipline;
        private VictoryPoints m_victoryPoint;
        private Good m_good;
        private GreenCard m_greenCard;
    }
}
