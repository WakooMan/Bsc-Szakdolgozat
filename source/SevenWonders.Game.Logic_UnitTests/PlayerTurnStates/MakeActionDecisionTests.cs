using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Ages;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.GameStructures;
using SevenWonders.Game.Logic.Handlers;
using SevenWonders.Game.Logic.PlayerActions;
using SevenWonders.Game.Logic.PlayerTurnStates;
using NSubstitute;

namespace GameLogic_UnitTests.PlayerTurnStates
{
    public class MakeActionDecisionTests
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
            m_eventManager = Substitute.For<IEventManager>();
            m_playerActionHandler = Substitute.For<IPlayerActionHandler>();
            m_gameContext = Substitute.For<IGameContext>();
            m_gameContext.PlayerActionHandler.Returns(m_playerActionHandler);
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_gameContext.AgeHandler.Returns(m_ageHandler);
            m_gameContext.EventManager.Returns(m_eventManager);
            m_makeActionDecision = new MakeActionDecision(m_gameContext);
        }

        [Test]
        public void When_ExecuteTurnState_Called()
        {
            m_cardComposition.AvailableCards.Returns([]);

            m_makeActionDecision.ExecuteTurnState();

            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnCardUnpicked>>());
            m_playerActionHandler.Received(1).HandlePlayerActionsCompleted(m_gameContext, m_player, Arg.Any<ICollection<IPlayerAction>>());
            m_eventManager.Received(1).Unsubscribe(Arg.Any<Action<OnCardUnpicked>>());
        }

        [Test]
        public void When_GetNextTurnState_Called_And_GoToPrevState_Is_True()
        {
            m_makeActionDecision.GoToPrevState = true;
            var turnState = m_makeActionDecision.GetNextTurnState();
            Assert.That(turnState is PickCardState, Is.True);
        }

        [Test]
        public void When_GetNextTurnState_Called_And_GoToPrevState_Is_False()
        {
            var turnState = m_makeActionDecision.GetNextTurnState();
            Assert.That(turnState is EndTurn, Is.True);
        }

        private IAgeBase m_age;
        private ICardComposition m_cardComposition;
        private Player m_player;
        private IEventManager m_eventManager;
        private IAgeHandler m_ageHandler;
        private ITurnHandler m_turnHandler;
        private IPlayerActionHandler m_playerActionHandler;
        private IGameContext m_gameContext;
        private MakeActionDecision m_makeActionDecision;
    }
}
