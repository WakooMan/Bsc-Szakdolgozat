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
    public class NewTurnTests
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
            m_newTurn = new NewTurn();
        }

        [Test]
        public void When_Clone_Called()
        {
            m_newTurn.AlreadyApplied = true;

            NewTurn newTurn = m_newTurn.Clone();

            Assert.That(newTurn, Is.Not.Null);
            Assert.That(m_newTurn, Is.Not.EqualTo(newTurn));
            Assert.That(newTurn.AlreadyApplied, Is.True);
        }

        [Test]
        public void When_Apply_Called_And_AlreadyApplied_False()
        {
            m_newTurn.AlreadyApplied = false;

            m_newTurn.Apply(m_gameContext);

            m_turnHandler.Received(1).ForceNewTurn();
            Assert.That(m_newTurn.AlreadyApplied, Is.True);
        }

        [Test]
        public void When_Apply_Called_And_AlreadyApplied_True()
        {
            m_newTurn.AlreadyApplied = true;

            m_newTurn.Apply(m_gameContext);

            m_turnHandler.DidNotReceive().ForceNewTurn();
            Assert.That(m_newTurn.AlreadyApplied, Is.True);
        }

        private IGameContext m_gameContext;
        private ITurnHandler m_turnHandler;
        private IEventManager m_eventManager;
        private Player m_player;
        private NewTurn m_newTurn;
    }
}
