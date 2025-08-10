using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Developments;
using GameLogic.Elements.Effects;
using GameLogic.Elements.Modifiers;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.Handlers;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using NSubstitute;
using SevenWonders.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class ChooseGoodTests
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
            m_chooseGood = new ChooseGood();
        }

        [Test]
        public void When_Clone_Called()
        {
            ChooseGood chooseGood = m_chooseGood.Clone();

            Assert.That(chooseGood, Is.Not.Null);
            Assert.That(m_chooseGood, Is.Not.EqualTo(chooseGood));
        }

        

        [Test]
        public void When_Apply_Called_And_Player_Is_Same_As_Arg()
        {
            TurnStarted turnStarted = new TurnStarted(m_player);
            m_eventManager.When((eventmgr) => eventmgr.Subscribe(Arg.Any<Action<TurnStarted>>())).Do((callinfo) => ((Action<TurnStarted>)callinfo[0])(turnStarted));

            m_chooseGood.Apply(m_gameContext);

            _ = m_turnHandler.Received(1).CurrentPlayer;
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<TurnStarted>>());
            m_playerActionReceiver.Received(1).ReceivePlayerAction(m_player, Arg.Any<ICollection<IPlayerAction>>());
            m_playerAction.Received(1).CanPerform(m_gameContext);
            m_playerAction.Received(1).DoPlayerAction(m_gameContext);
        }

        [Test]
        public void When_Apply_Called_And_Player_Is_Different_Than_Arg()
        {
            TurnStarted turnStarted = new TurnStarted(new Player());
            m_eventManager.When((eventmgr) => eventmgr.Subscribe(Arg.Any<Action<TurnStarted>>())).Do((callinfo) => ((Action<TurnStarted>)callinfo[0])(turnStarted));

            m_chooseGood.Apply(m_gameContext);

            _ = m_turnHandler.Received(1).CurrentPlayer;
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<TurnStarted>>());
            m_playerActionReceiver.DidNotReceive().ReceivePlayerAction(m_player, Arg.Any<ICollection<IPlayerAction>>());
            m_playerAction.DidNotReceive().CanPerform(m_gameContext);
            m_playerAction.DidNotReceive().DoPlayerAction(m_gameContext);
        }

        private IEventManager m_eventManager;
        private IGameContext m_gameContext;
        private ITurnHandler m_turnHandler;
        private IPlayerAction m_playerAction;
        private IPlayerActionReceiver m_playerActionReceiver;
        private Player m_player;
        private ChooseGood m_chooseGood;
    }
}
