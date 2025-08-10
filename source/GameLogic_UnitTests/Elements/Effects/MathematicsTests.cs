using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Elements.Modifiers;
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
    public class MathematicsTests
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
            m_mathematics = new Mathematics();
            m_mathematics.VictoryPointsPerDevelopment.Points = 5;
        }

        [Test]
        public void When_Clone_Called()
        {
            Mathematics mathematics = m_mathematics.Clone();

            Assert.That(mathematics, Is.Not.Null);
            Assert.That(m_mathematics, Is.Not.EqualTo(mathematics));
            Assert.That(mathematics.VictoryPointsPerDevelopment.Points, Is.EqualTo(m_mathematics.VictoryPointsPerDevelopment.Points));
        }

        [Test]
        public void When_Apply_Called()
        {
            m_player.Developments.AddRange([new Development(), new Development()]);
            OnGameEnded onGameEnded = new OnGameEnded([m_player]);
            m_eventManager.When((evt) => evt.Subscribe(Arg.Any<Action<OnGameEnded>>())).Do((callinfo) => ((Action<OnGameEnded>)callinfo[0])(onGameEnded));

            m_mathematics.Apply(m_gameContext);

            _ = m_turnHandler.Received(1).CurrentPlayer;
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnGameEnded>>());
            Assert.That(onGameEnded.VictoryPoints[m_player], Is.EqualTo(10));

        }

        private IGameContext m_gameContext;
        private ITurnHandler m_turnHandler;
        private IEventManager m_eventManager;
        private Player m_player;
        private Mathematics m_mathematics;
    }
}
