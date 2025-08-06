using GameLogic.Ages;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Goods;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.GameCards
{
    public class YellowCardTests
    {
        [SetUp]
        public void Setup()
        {
            m_good = Substitute.For<Good>();
            m_effect = Substitute.For<Effect>();
            m_yellowCard = new YellowCard()
            {
                Name = "testName",
                PreviousBuilding = "testPrevious",
                GoodCost = new List<Good> { m_good },
                Age = AgesEnum.I,
                Effects = new List<Effect>() { m_effect }
            };
        }

        [Test]
        public void When_Initialized_With_Default_Constructor()
        {
            m_yellowCard = new YellowCard();

            Assert.That(m_yellowCard, Is.Not.Null);
            Assert.That(m_yellowCard.BuildingType, Is.EqualTo(typeof(YellowCard).Name));
            Assert.That(m_yellowCard.Name, Is.EqualTo(string.Empty));
            Assert.That(m_yellowCard.PreviousBuilding, Is.EqualTo(string.Empty));
            Assert.That(m_yellowCard.GoodCost, Is.Not.Null);
            Assert.That(m_yellowCard.GoodCost.Count, Is.EqualTo(0));
            Assert.That(m_yellowCard.Age, Is.EqualTo(AgesEnum.None));

            Assert.That(m_yellowCard.Effects, Is.Not.Null);
            Assert.That(m_yellowCard.Effects.Count, Is.EqualTo(0));
        }

        [Test]
        public void When_Clone_Called()
        {
            YellowCard yellowCard = m_yellowCard.Clone();

            Assert.That(yellowCard, Is.Not.Null);
            Assert.That(yellowCard.Equals(m_yellowCard), Is.False);
            Assert.That(yellowCard.Name, Is.EqualTo(m_yellowCard.Name));
            Assert.That(yellowCard.PreviousBuilding, Is.EqualTo(m_yellowCard.PreviousBuilding));
            Assert.That(yellowCard.GoodCost, Is.Not.Null);
            Assert.That(yellowCard.GoodCost.Count, Is.EqualTo(m_yellowCard.GoodCost.Count));
            m_good.Received(1).Clone();
            Assert.That(yellowCard.Age, Is.EqualTo(m_yellowCard.Age));

            Assert.That(yellowCard.Effects, Is.Not.Null);
            Assert.That(yellowCard.Effects.Count, Is.EqualTo(m_yellowCard.Effects.Count));
            m_effect.Received(1).Clone();
        }

        private Effect m_effect;
        private Good m_good;
        private YellowCard m_yellowCard;
    }
}
