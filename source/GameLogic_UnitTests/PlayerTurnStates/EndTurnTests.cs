using GameLogic.PlayerTurnStates;

namespace GameLogic_UnitTests.PlayerTurnStates
{
    public class EndTurnTests
    {
        [SetUp]
        public void Setup()
        {
            m_endTurn = new EndTurn();
        }

        [Test]
        public void When_ExecuteTurnState_Called()
        {
            Assert.Throws<NotImplementedException>(m_endTurn.ExecuteTurnState);
        }

        [Test]
        public void When_GetNextTurnState_Called()
        {
            var turnState = m_endTurn.GetNextTurnState();
            Assert.That(m_endTurn, Is.EqualTo(turnState));
        }

        private EndTurn m_endTurn;
    }
}
