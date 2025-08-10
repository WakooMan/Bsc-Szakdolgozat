using GameLogic;
using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.GameStates;
using GameLogic.GameStructures;
using GameLogic.Handlers;
using GameLogic.Interfaces;
using NSubstitute;

namespace GameLogic_UnitTests.GameStates
{
    public class PlayingStateTests
    {
        [SetUp]
        public void Setup()
        {
            m_player = new Player();
            m_eventManager = Substitute.For<IEventManager>();
            m_gameContext = Substitute.For<IGameContext>();
            m_age = Substitute.For<IAgeBase>();
            m_ageHandler = Substitute.For<IAgeHandler>();
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_cardComposition = Substitute.For<ICardComposition>();
            m_age.Composition.Returns(m_cardComposition);
            m_turnHandler.CurrentPlayer.Returns(m_player);
            m_turnHandler.OpponentPlayer.Returns(new Player());
            m_ageHandler.CurrentAge.Returns(m_age);
            m_playerActionReceiver = Substitute.For<IPlayerActionReceiver>();
            m_gameContext.PlayerActionReceiver.Returns(m_playerActionReceiver);
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_gameContext.AgeHandler.Returns(m_ageHandler);
            m_gameContext.EventManager.Returns(m_eventManager);
            m_playingState = new PlayingState(m_gameContext);
        }

        [Test]
        public void When_DoStateAction_Called()
        {
            m_age.IsAgeOver.Returns(true);
            m_ageHandler.NextAge().Returns(false);

            m_playingState.DoStateAction();

            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnMilitaryTokenReachedThreshold>>());
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<MilitaryVictory>>());
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<ScientificVictory>>());
            m_turnHandler.Received(1).NextPlayer();
            m_eventManager.Received(1).Unsubscribe(Arg.Any<Action<OnMilitaryTokenReachedThreshold>>());
            m_eventManager.Received(1).Unsubscribe(Arg.Any<Action<MilitaryVictory>>());
            m_eventManager.Received(1).Unsubscribe(Arg.Any<Action<ScientificVictory>>());
            m_eventManager.Received(1).Publish(Arg.Any<OnGameEnded>());


        }

        [Test]
        public void When_GetNextState_Called()
        {
            var turnState = m_playingState.GetNextState();
            Assert.That(turnState is EndGameState, Is.True);
        }

        private IAgeBase m_age;
        private ICardComposition m_cardComposition;
        private Player m_player;
        private IAgeHandler m_ageHandler;
        private ITurnHandler m_turnHandler;
        private IEventManager m_eventManager;
        private IPlayerActionReceiver m_playerActionReceiver;
        private IGameContext m_gameContext;
        private PlayingState m_playingState;
    }
}
