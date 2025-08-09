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
    public class GetMoneyForWonderTests
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
            m_getMoneyForWonders = new GetMoneyForWonders();
        }

        [Test]
        public void When_Clone_Called()
        {
            m_getMoneyForWonders.MoneyPerWonder = 5;
            GetMoneyForWonders getMoneyForWonders = m_getMoneyForWonders.Clone();

            Assert.That(getMoneyForWonders, Is.Not.Null);
            Assert.That(m_getMoneyForWonders, Is.Not.EqualTo(getMoneyForWonders));
            Assert.That(getMoneyForWonders.MoneyPerWonder, Is.EqualTo(m_getMoneyForWonders.MoneyPerWonder));
        }



        [TestCase(5, 2, 10)]
        [TestCase(4, 3, 12)]
        [TestCase(5, 1, 5)]
        [TestCase(3, 1, 3)]
        public void When_Apply_Called(int moneyPerWonder, int wondersBuilt, int expectedMoney)
        {
            for (int i = 0; i < wondersBuilt; i++)
            {
                m_player.Wonders.Add(new Wonder() { HasBeenBuilt = true });
            }

            m_getMoneyForWonders.MoneyPerWonder = moneyPerWonder;

            m_getMoneyForWonders.Apply(m_gameContext);

            _ = m_turnHandler.Received(1).CurrentPlayer;
            Assert.That(m_player.Money, Is.EqualTo(Math.Max(0, expectedMoney)));
        }

        private IGameContext m_gameContext;
        private ITurnHandler m_turnHandler;
        private Player m_player;
        private GetMoneyForWonders m_getMoneyForWonders;
    }
}
