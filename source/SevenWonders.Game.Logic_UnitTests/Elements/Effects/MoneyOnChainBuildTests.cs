using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Effects;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.Handlers;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class MoneyOnChainBuildTests
    {
        [SetUp]
        public void Setup()
        {
            m_gameContext = Substitute.For<IGameContext>();
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_eventManager = Substitute.For<IEventManager>();
            m_player = new Player();
            m_turnHandler.CurrentPlayer.Returns(m_player);
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_gameContext.EventManager.Returns(m_eventManager);
            m_moneyOnChainBuild = new MoneyOnChainBuild();
            m_moneyOnChainBuild.MoneyToGet.Money = 5;
        }

        [Test]
        public void When_Clone_Called()
        {
            MoneyOnChainBuild moneyOnChainBuild = m_moneyOnChainBuild.Clone();

            Assert.That(moneyOnChainBuild, Is.Not.Null);
            Assert.That(m_moneyOnChainBuild, Is.Not.EqualTo(moneyOnChainBuild));
            Assert.That(moneyOnChainBuild.MoneyToGet.Money, Is.EqualTo(m_moneyOnChainBuild.MoneyToGet.Money));
        }

        [Test]
        public void When_Apply_Called_When_Player_Is_Builder_And_ChainBuild_True()
        {
            OnCardBuilt onCardBuilt = new OnCardBuilt(Substitute.For<Card>(), m_player, 0, true);

            m_moneyOnChainBuild.Apply(m_gameContext, m_player, new Player());

            m_player.OnBuildCard(onCardBuilt);
            Assert.That(m_player.Money, Is.EqualTo(5));
        }

        [Test]
        public void When_Apply_Called_When_Player_Is_Builder_And_ChainBuild_False()
        {
            OnCardBuilt onCardBuilt = new OnCardBuilt(Substitute.For<Card>(), m_player, 0, false);

            m_moneyOnChainBuild.Apply(m_gameContext, m_player, new Player());

            m_player.OnBuildCard(onCardBuilt);
            Assert.That(m_player.Money, Is.EqualTo(0));
        }

        [Test]
        public void When_Apply_Called_When_Player_Is_Not_Builder_And_ChainBuild_True()
        {
            OnCardBuilt onCardBuilt = new OnCardBuilt(Substitute.For<Card>(), new Player(), 0, true);

            m_moneyOnChainBuild.Apply(m_gameContext, m_player, new Player());

            m_player.OnBuildCard(onCardBuilt);
            Assert.That(m_player.Money, Is.EqualTo(5));
        }

        private IGameContext m_gameContext;
        private ITurnHandler m_turnHandler;
        private IEventManager m_eventManager;
        private Player m_player;
        private MoneyOnChainBuild m_moneyOnChainBuild;
    }
}
