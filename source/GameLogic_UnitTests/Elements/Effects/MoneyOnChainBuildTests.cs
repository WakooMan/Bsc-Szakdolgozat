using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.Handlers;
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
            m_eventManager.When((evt) => evt.Subscribe(Arg.Any<Action<OnCardBuilt>>())).Do((callinfo) => ((Action<OnCardBuilt>)callinfo[0])(onCardBuilt));

            m_moneyOnChainBuild.Apply(m_gameContext);

            _ = m_turnHandler.Received(1).CurrentPlayer;
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnCardBuilt>>());
            Assert.That(m_player.Money, Is.EqualTo(5));
        }

        [Test]
        public void When_Apply_Called_When_Player_Is_Builder_And_ChainBuild_False()
        {
            OnCardBuilt onCardBuilt = new OnCardBuilt(Substitute.For<Card>(), m_player, 0, false);
            m_eventManager.When((evt) => evt.Subscribe(Arg.Any<Action<OnCardBuilt>>())).Do((callinfo) => ((Action<OnCardBuilt>)callinfo[0])(onCardBuilt));

            m_moneyOnChainBuild.Apply(m_gameContext);

            _ = m_turnHandler.Received(1).CurrentPlayer;
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnCardBuilt>>());
            Assert.That(m_player.Money, Is.EqualTo(0));
        }

        [Test]
        public void When_Apply_Called_When_Player_Is_Not_Builder_And_ChainBuild_True()
        {
            OnCardBuilt onCardBuilt = new OnCardBuilt(Substitute.For<Card>(), new Player(), 0, true);
            m_eventManager.When((evt) => evt.Subscribe(Arg.Any<Action<OnCardBuilt>>())).Do((callinfo) => ((Action<OnCardBuilt>)callinfo[0])(onCardBuilt));

            m_moneyOnChainBuild.Apply(m_gameContext);

            _ = m_turnHandler.Received(1).CurrentPlayer;
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnCardBuilt>>());
            Assert.That(m_player.Money, Is.EqualTo(0));
        }

        private IGameContext m_gameContext;
        private ITurnHandler m_turnHandler;
        private IEventManager m_eventManager;
        private Player m_player;
        private MoneyOnChainBuild m_moneyOnChainBuild;
    }
}
