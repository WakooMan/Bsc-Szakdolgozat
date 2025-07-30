using GameLogic.Elements;
using GameLogic.Events;
using GameLogic.Handlers;
using GameLogic.Interfaces;
using NSubstitute;

namespace GameLogic_UnitTests.Handlers
{
    public class TurnHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            m_player1 = new Player("Test1");
            m_player2 = new Player("Test2");
            m_eventManager = Substitute.For<IEventManager>();
            m_turnHandler = new TurnHandler(m_eventManager);
            m_turnHandler.Initialize([m_player1, m_player2]);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new TurnHandler(null));
        }

        [Test]
        public void When_Initialized_Called_With_Less_Than_Two_Players()
        {
            Assert.Throws<ArgumentException>(() => m_turnHandler.Initialize(Array.Empty<Player>()));
            Assert.Throws<ArgumentException>(() => m_turnHandler.Initialize([m_player1]));
        }

        [Test]
        public void When_Not_Initialized()
        {
            m_turnHandler = new TurnHandler(m_eventManager);
            Assert.Throws<InvalidOperationException>(() => { Player player = m_turnHandler.CurrentPlayer; });
            Assert.Throws<InvalidOperationException>(() => { Player player = m_turnHandler.OpponentPlayer; });
            Assert.Throws<InvalidOperationException>(() => m_turnHandler.NextPlayer());
            Assert.Throws<InvalidOperationException>(() => m_turnHandler.ForceNewTurn());
        }

        [Test]
        public void When_Initialized()
        {
            Assert.That(m_turnHandler.CurrentPlayer == m_player1, Is.True);
        }

        [Test]
        public void When_NextPlayer_Called_Success()
        {
            m_turnHandler.NextPlayer();
            Assert.That(m_turnHandler.CurrentPlayer == m_player2, Is.True);
            m_eventManager.Received(1).Publish(GameEventType.TurnStarted, Arg.Any<TurnStarted>());
            m_eventManager.Received(1).Publish(GameEventType.TurnEnded, Arg.Any<TurnEnded>());
            m_turnHandler.NextPlayer();
            Assert.That(m_turnHandler.CurrentPlayer == m_player1, Is.True);
            m_eventManager.Received(2).Publish(GameEventType.TurnStarted, Arg.Any<TurnStarted>());
            m_eventManager.Received(2).Publish(GameEventType.TurnEnded, Arg.Any<TurnEnded>());
            m_turnHandler.NextPlayer();
            Assert.That(m_turnHandler.CurrentPlayer == m_player2, Is.True);
            m_eventManager.Received(3).Publish(GameEventType.TurnStarted, Arg.Any<TurnStarted>());
            m_eventManager.Received(3).Publish(GameEventType.TurnEnded, Arg.Any<TurnEnded>());
        }

        private TurnHandler m_turnHandler;
        private Player m_player1;
        private Player m_player2;
        private IEventManager m_eventManager;
    }
}
