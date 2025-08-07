using GameLogic;
using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.GameStructures;
using GameLogic.Handlers;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using GameLogic.PlayerTurnStates;
using NSubstitute;

namespace GameLogic_UnitTests.PlayerTurnStates
{
    public class PickCardStateTests
    {
        [SetUp]
        public void Setup()
        {
            m_player = new Player();
            m_age = Substitute.For<IAgeBase>();
            m_ageHandler = Substitute.For<IAgeHandler>();
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_cardComposition = Substitute.For<ICardComposition>();
            m_age.Composition.Returns(m_cardComposition);
            m_turnHandler.CurrentPlayer.Returns(m_player);
            m_ageHandler.CurrentAge.Returns(m_age);
            m_playerActionReceiver = Substitute.For<IPlayerActionReceiver>();
            m_gameContext = Substitute.For<IGameContext>();
            m_gameContext.PlayerActionReceiver.Returns(m_playerActionReceiver);
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_gameContext.AgeHandler.Returns(m_ageHandler);
            m_pickCardState = new PickCardState(m_gameContext);
        }

        [Test]
        public void When_ExecuteTurnState_Called()
        {
            IPlayerAction playerAction = Substitute.For<IPlayerAction>();
            m_cardComposition.AvailableCards.Returns([]);
            m_playerActionReceiver.ReceivePlayerAction(Arg.Any<Player>(),Arg.Any<ICollection<IPlayerAction>>()).Returns(playerAction);

            m_pickCardState.ExecuteTurnState();

            m_playerActionReceiver.Received(1).ReceivePlayerAction(Arg.Any<Player>(), Arg.Any<ICollection<IPlayerAction>>());
            playerAction.Received(1).DoPlayerAction();
        }

        [Test]
        public void When_GetNextTurnState_Called()
        {
            var turnState = m_pickCardState.GetNextTurnState();
            Assert.That(turnState is MakeActionDecision, Is.True);
        }

        private IAgeBase m_age;
        private ICardComposition m_cardComposition;
        private Player m_player;
        private IAgeHandler m_ageHandler;
        private ITurnHandler m_turnHandler;
        private IPlayerActionReceiver m_playerActionReceiver;
        private IGameContext m_gameContext;
        private PickCardState m_pickCardState;
    }
}
