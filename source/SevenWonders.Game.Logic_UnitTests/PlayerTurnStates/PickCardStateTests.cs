using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Ages;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.GameStructures;
using SevenWonders.Game.Logic.Handlers;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.Game.Logic.PlayerActions;
using SevenWonders.Game.Logic.PlayerTurnStates;
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
            m_playerActionHandler = Substitute.For<IPlayerActionHandler>();
            m_gameContext = Substitute.For<IGameContext>();
            m_gameContext.PlayerActionHandler.Returns(m_playerActionHandler);
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_gameContext.AgeHandler.Returns(m_ageHandler);
            m_pickCardState = new PickCardState(m_gameContext);
        }

        [Test]
        public void When_ExecuteTurnState_Called()
        {
            IPlayerAction playerAction = Substitute.For<IPlayerAction>();
            playerAction.CanPerform(m_gameContext).Returns(true);
            m_cardComposition.AvailableCards.Returns([]);
            m_playerActionHandler.HandlePlayerActions(m_gameContext, Arg.Any<Player>(),Arg.Any<ICollection<IPlayerAction>>()).Returns((true, playerAction));

            m_pickCardState.ExecuteTurnState();

            m_playerActionHandler.Received(1).HandlePlayerActions(m_gameContext, Arg.Any<Player>(), Arg.Any<ICollection<IPlayerAction>>());
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
        private IPlayerActionHandler m_playerActionHandler;
        private IGameContext m_gameContext;
        private PickCardState m_pickCardState;
    }
}
