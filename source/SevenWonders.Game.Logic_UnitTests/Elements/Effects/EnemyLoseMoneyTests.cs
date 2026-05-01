using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Effects;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.Handlers;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.Game.Logic.PlayerActions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class EnemyLoseMoneyTests
    {
        [SetUp]
        public void Setup()
        {
            m_gameContext = Substitute.For<IGameContext>();
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_player = new Player();
            m_opponent = new Player();
            m_turnHandler.CurrentPlayer.Returns(m_player);
            m_turnHandler.OpponentPlayer.Returns(m_opponent);
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_enemyLoseMoney = new EnemyLoseMoney();
        }

        [Test]
        public void When_Clone_Called()
        {
            m_enemyLoseMoney.Money = 5;
            EnemyLoseMoney enemyLoseMoney = m_enemyLoseMoney.Clone();

            Assert.That(enemyLoseMoney, Is.Not.Null);
            Assert.That(m_enemyLoseMoney, Is.Not.EqualTo(enemyLoseMoney));
            Assert.That(enemyLoseMoney.Money, Is.EqualTo(m_enemyLoseMoney.Money));
        }



        [TestCase(5,4)]
        [TestCase(5, 5)]
        [TestCase(5, 6)]
        public void When_Apply_Called(int opponentMoney, int loseNum)
        {
            m_opponent.Money = opponentMoney;
            m_enemyLoseMoney.Money = loseNum;

            m_enemyLoseMoney.Apply(m_gameContext, m_player, m_opponent);

            Assert.That(m_opponent.Money, Is.EqualTo(Math.Max(0, opponentMoney - loseNum)));
        }

        private IGameContext m_gameContext;
        private ITurnHandler m_turnHandler;
        private Player m_player;
        private Player m_opponent;
        private EnemyLoseMoney m_enemyLoseMoney;
    }
}
