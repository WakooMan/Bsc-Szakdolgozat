using GameLogic.Elements;
using GameLogic.Handlers;

namespace GameLogic_UnitTests.Handlers
{
    public class TurnHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            m_player1 = new Player("Test1");
            m_player2 = new Player("Test2");
            m_turnHandler = new TurnHandler(new Player[] { m_player1, m_player2 });
            m_OnPlayerTurnEventCalled = 0;
            m_turnHandler.OnPlayerTurn += (player) => m_OnPlayerTurnEventCalled++;
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new TurnHandler(null));
        }

        [Test]
        public void When_Constructor_Called_With_Less_Than_Two_Players()
        {
            Assert.Throws<ArgumentException>(() => new TurnHandler(new Player[] { }));
            Assert.Throws<ArgumentException>(() => new TurnHandler(new Player[] { m_player1 }));
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
            Assert.That(m_OnPlayerTurnEventCalled == 1, Is.True);
            m_turnHandler.NextPlayer();
            Assert.That(m_turnHandler.CurrentPlayer == m_player1, Is.True);
            Assert.That(m_OnPlayerTurnEventCalled == 2, Is.True);
            m_turnHandler.NextPlayer();
            Assert.That(m_turnHandler.CurrentPlayer == m_player2, Is.True);
            Assert.That(m_OnPlayerTurnEventCalled == 3, Is.True);
        }

        private TurnHandler m_turnHandler;
        private int m_OnPlayerTurnEventCalled;
        private Player m_player1;
        private Player m_player2;
    }
}
