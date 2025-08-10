using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;
using GameLogic.Handlers;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class GetMoneyTests
    {
        [SetUp]
        public void Setup()
        {
            m_gameContext = Substitute.For<IGameContext>();
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_player = new Player() { Wonders = [new Wonder(), new Wonder(), new Wonder()] };
            m_player.Cards.AddRange(new RedCard(), new RedCard(), new GreenCard(), new YellowCard());
            m_turnHandler.CurrentPlayer.Returns(m_player);
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_getMoney = new GetMoney();
        }

        [Test]
        public void When_Clone_Called()
        {
            m_getMoney.Money = 5;
            GetMoney getMoney = m_getMoney.Clone();

            Assert.That(getMoney, Is.Not.Null);
            Assert.That(m_getMoney, Is.Not.EqualTo(getMoney));
            Assert.That(getMoney.Money, Is.EqualTo(m_getMoney.Money));
        }



        [TestCase(5, 5)]
        [TestCase(4, 4)]
        [TestCase(5, 5)]
        [TestCase(3, 3)]
        [TestCase(-1, 0)]
        [TestCase(0, 0)]
        public void When_Apply_Called(int moneyToGet, int expectedMoney)
        {
            m_getMoney.Money = moneyToGet;

            m_getMoney.Apply(m_gameContext);

            Assert.That(m_player.Money, Is.EqualTo(Math.Max(0, expectedMoney)));
        }

        private IGameContext m_gameContext;
        private ITurnHandler m_turnHandler;
        private Player m_player;
        private GetMoney m_getMoney;
    }
}
