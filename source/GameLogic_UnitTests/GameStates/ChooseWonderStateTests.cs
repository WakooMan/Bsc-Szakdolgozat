using GameLogic;
using GameLogic.GameStates;
using GameLogic.Handlers;
using NSubstitute;

namespace GameLogic_UnitTests.GameStates
{
    public class ChooseWonderStateTests
    {
        [SetUp]
        public void Setup()
        {
            m_chooseWonderHandler = Substitute.For<IChooseWonderHandler>();
            m_gameContext = Substitute.For<IGameContext>();
            m_gameContext.ChooseWonderHandler.Returns(m_chooseWonderHandler);
            m_chooseWonderState = new ChooseWonderState(m_gameContext);
        }

        [Test]
        public void When_DoStateAction_Called_And_WonderChosen_Returns_True()
        {
            m_chooseWonderHandler.WondersChosen.Returns(true);

            m_chooseWonderState.DoStateAction();

            m_chooseWonderHandler.DidNotReceive().ChooseWonder();
        }

        [Test]
        public void When_DoStateAction_Called_And_WonderChosen_Returns_False_Once()
        {
            m_chooseWonderHandler.WondersChosen.Returns(false, true);

            m_chooseWonderState.DoStateAction();

            m_chooseWonderHandler.Received(1).ChooseWonder();
        }

        [Test]
        public void When_GetNextState_Called()
        {
            var turnState = m_chooseWonderState.GetNextState();
            Assert.That(turnState is PlayingState, Is.True);
        }

        private IChooseWonderHandler m_chooseWonderHandler;
        private IGameContext m_gameContext;
        private ChooseWonderState m_chooseWonderState;
    }
}
