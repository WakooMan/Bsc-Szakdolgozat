using GameLogic;
using GameLogic.Elements.Effects;
using GameLogic.Elements.Military;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Military
{
    public class MilitaryCardTests
    {
        [SetUp]
        public void Setup()
        {
            m_card = new MilitaryCard();
        }

        [Test]
        public void When_Default_Constructor_Called()
        {
            Assert.That(m_card.EnemyLoseMoney, Is.Not.Null);
            Assert.That(m_card.EnemyLoseMoney.Money, Is.EqualTo(0));
            Assert.That(m_card.VictoryPoints, Is.Not.Null);
            Assert.That(m_card.VictoryPoints.Points, Is.EqualTo(0));
            Assert.That(m_card.IndexEnd, Is.EqualTo(0));
            Assert.That(m_card.IndexStart, Is.EqualTo(0));
        }

        [Test]
        public void When_Apply_Called()
        {
            EnemyLoseMoney enemyLoseMoney = Substitute.For<EnemyLoseMoney>();
            VictoryPoints victoryPoints = Substitute.For<VictoryPoints>();
            m_card.EnemyLoseMoney = enemyLoseMoney;
            m_card.VictoryPoints = victoryPoints;
            IGameContext gameContext = Substitute.For<IGameContext>();

            m_card.Apply(gameContext);

            enemyLoseMoney.Received(1).Apply(gameContext);
            victoryPoints.Apply(gameContext);
        }

        private MilitaryCard m_card;
    }
}
