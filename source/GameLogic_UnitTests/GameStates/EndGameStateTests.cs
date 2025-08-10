using GameLogic.GameStates;

namespace GameLogic_UnitTests.GameStates
{
    public class EndGameStateTests
    {
        [SetUp]
        public void Setup()
        {
            m_endGameState = new EndGameState();
        }

        [Test]
        public void When_DoStateAction_Called()
        {
            Assert.Throws<NotImplementedException>(m_endGameState.DoStateAction);
        }

        [Test]
        public void When_GetNextState_Called()
        {
            var turnState = m_endGameState.GetNextState();
            Assert.That(m_endGameState, Is.EqualTo(turnState));
        }


        private EndGameState m_endGameState;
    }
}
