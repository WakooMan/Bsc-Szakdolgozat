using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.Handlers;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class EconomicsTests
    {
        [SetUp]
        public void Setup()
        {
            m_gameContext = Substitute.For<IGameContext>();
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_playerAction = Substitute.For<IPlayerAction>();
            m_playerActionReceiver = Substitute.For<IPlayerActionReceiver>();
            m_eventManager = Substitute.For<IEventManager>();
            m_player = new Player();
            m_turnHandler.CurrentPlayer.Returns(m_player);
            m_playerAction.CanPerform(m_gameContext).Returns(true);
            m_playerActionReceiver.ReceivePlayerAction(m_player, Arg.Any<ICollection<IPlayerAction>>()).Returns(m_playerAction);
            m_gameContext.PlayerActionReceiver.Returns(m_playerActionReceiver);
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_gameContext.EventManager.Returns(m_eventManager);
            m_economics = new Economics();
        }

        [Test]
        public void When_Clone_Called()
        {
            Economics economics = m_economics.Clone();

            Assert.That(economics, Is.Not.Null);
            Assert.That(m_economics, Is.Not.EqualTo(economics));
        }



        [TestCase(10, 4)]
        [TestCase(4, 4)]
        [TestCase(4, 5)]
        public void When_Apply_Called_And_Player_Is_Same_As_Arg(int buildCost, int moneyCost)
        {
            Card card = new RedCard() { MoneyCost = moneyCost };
            OnCardBuilt onCardBuilt = new OnCardBuilt(card, m_player, buildCost, true);
            m_eventManager.When((eventmgr) => eventmgr.Subscribe(Arg.Any<Action<OnCardBuilt>>())).Do((callinfo) => ((Action<OnCardBuilt>)callinfo[0])(onCardBuilt));

            m_economics.Apply(m_gameContext);

            _ = m_turnHandler.Received(1).CurrentPlayer;
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnCardBuilt>>());
            Assert.That(m_player.Money, Is.EqualTo(0));
        }

        [TestCase(10, 4)]
        [TestCase(4, 4)]
        [TestCase(4, 5)]
        public void When_Apply_Called_And_Player_Is_Different_Than_Arg(int buildCost, int moneyCost)
        {
            Card card = new RedCard() { MoneyCost = moneyCost };
            OnCardBuilt onCardBuilt = new OnCardBuilt(card, new Player(), buildCost, true);
            m_eventManager.When((eventmgr) => eventmgr.Subscribe(Arg.Any<Action<OnCardBuilt>>())).Do((callinfo) => ((Action<OnCardBuilt>)callinfo[0])(onCardBuilt));

            m_economics.Apply(m_gameContext);

            _ = m_turnHandler.Received(1).CurrentPlayer;
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnCardBuilt>>());
            Assert.That(m_player.Money, Is.EqualTo(Math.Max(0, buildCost - card.MoneyCost)));
        }

        private IEventManager m_eventManager;
        private IGameContext m_gameContext;
        private ITurnHandler m_turnHandler;
        private IPlayerAction m_playerAction;
        private IPlayerActionReceiver m_playerActionReceiver;
        private Player m_player;
        private Economics m_economics;
    }
}
