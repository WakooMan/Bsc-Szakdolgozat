using NSubstitute;
using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Handlers;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.Game.Logic.PlayerActions;

namespace GameLogic_UnitTests.Handlers
{
    [TestFixture]
    public class PlayerActionHandlerTests
    {
        private PlayerActionHandler m_handler;
        private IGameContext m_gameContext;
        private IPlayerActionReceiver m_receiver;
        private Player m_player;

        [SetUp]
        public void Setup()
        {
            m_handler = new PlayerActionHandler();
            m_gameContext = Substitute.For<IGameContext>();
            m_receiver = Substitute.For<IPlayerActionReceiver>();
            m_player = new Player(m_receiver, "TestPlayer", 1, 10);
        }

        [Test]
        public void When_HandlePlayerActions_Called_NullReceiver_ShouldThrow()
        {
            var player = new Player { Name = "NoReceiver" };
            var action = Substitute.For<IPlayerAction>();

            Assert.Throws<InvalidOperationException>(() =>
                m_handler.HandlePlayerActions(m_gameContext, player, new[] { action }));
        }

        [Test]
        public void When_HandlePlayerActions_Called_CanPerform_ShouldExecuteAction()
        {
            var action = Substitute.For<IPlayerAction>();
            action.CanPerform(m_gameContext).Returns(true);
            action.DoPlayerAction(m_gameContext).Returns(true);

            var wrapper = new PlayerActionWrapper(action, true);
            m_receiver.ReceivePlayerAction(m_player, Arg.Any<ICollection<PlayerActionWrapper>>()).Returns(wrapper);

            var result = m_handler.HandlePlayerActions(m_gameContext, m_player, new[] { action });

            Assert.Multiple(() =>
            {
                Assert.That(result.completed, Is.True);
                Assert.That(result.playerAction, Is.EqualTo(action));
            });
        }

        [Test]
        public void When_HandlePlayerActions_Called_CannotPerform_ShouldReturnFalse()
        {
            var action = Substitute.For<IPlayerAction>();
            action.CanPerform(m_gameContext).Returns(false);

            var wrapper = new PlayerActionWrapper(action, false);
            m_receiver.ReceivePlayerAction(m_player, Arg.Any<ICollection<PlayerActionWrapper>>()).Returns(wrapper);

            var result = m_handler.HandlePlayerActions(m_gameContext, m_player, new[] { action });

            Assert.Multiple(() =>
            {
                Assert.That(result.completed, Is.False);
                Assert.That(result.playerAction, Is.Null);
            });
        }

        [Test]
        public void When_HandlePlayerActionsCompleted_Called_NullReceiver_ShouldThrow()
        {
            var player = new Player { Name = "NoReceiver" };
            var action = Substitute.For<IPlayerAction>();

            Assert.Throws<InvalidOperationException>(() =>
                m_handler.HandlePlayerActionsCompleted(m_gameContext, player, new[] { action }));
        }

        [Test]
        public void When_HandlePlayerActionsCompleted_Called_PerformableAction_ShouldReturnAction()
        {
            var action = Substitute.For<IPlayerAction>();
            action.CanPerform(m_gameContext).Returns(true);
            action.DoPlayerAction(m_gameContext).Returns(true);

            var wrapper = new PlayerActionWrapper(action, true);
            m_receiver.ReceivePlayerAction(m_player, Arg.Any<ICollection<PlayerActionWrapper>>()).Returns(wrapper);

            var result = m_handler.HandlePlayerActionsCompleted(m_gameContext, m_player, new[] { action });

            Assert.That(result, Is.EqualTo(action));
        }

        [Test]
        public void When_HandlePlayerAction_Called_CanPerform_ShouldReturnTrue()
        {
            var action = Substitute.For<IPlayerAction>();
            action.CanPerform(m_gameContext).Returns(true);
            action.DoPlayerAction(m_gameContext).Returns(true);

            var result = m_handler.HandlePlayerAction(m_gameContext, m_player, action);

            Assert.That(result, Is.True);
        }

        [Test]
        public void When_HandlePlayerAction_Called_CannotPerform_ShouldReturnFalse()
        {
            var action = Substitute.For<IPlayerAction>();
            action.CanPerform(m_gameContext).Returns(false);

            var result = m_handler.HandlePlayerAction(m_gameContext, m_player, action);

            Assert.That(result, Is.False);
            action.DidNotReceive().DoPlayerAction(m_gameContext);
        }
    }
}
