using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;
using GameLogic.Handlers;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class LawTests
    {
        [SetUp]
        public void Setup()
        {
            m_gameContext = Substitute.For<IGameContext>();
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_playerAction = Substitute.For<IPlayerAction>();
            m_playerActionReceiver = Substitute.For<IPlayerActionReceiver>();
            m_player = new Player();
            m_playerAction.CanPerform(m_gameContext).Returns(true);
            m_playerActionReceiver.ReceivePlayerAction(m_player, Arg.Any<ICollection<IPlayerAction>>()).Returns(m_playerAction);
            m_turnHandler.CurrentPlayer.Returns(m_player);
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_gameContext.PlayerActionReceiver.Returns(m_playerActionReceiver);
            m_law = new Law();
        }

        [Test]
        public void When_Clone_Called()
        {
            Law law = m_law.Clone();

            Assert.That(law, Is.Not.Null);
            Assert.That(m_law, Is.Not.EqualTo(law));
        }

        [Test]
        public void When_Apply_Called()
        {

            m_law.Apply(m_gameContext);

            m_playerActionReceiver.Received(1).ReceivePlayerAction(m_player, Arg.Any<ICollection<IPlayerAction>>());
            m_playerAction.Received(1).CanPerform(m_gameContext);
            m_playerAction.Received(1).DoPlayerAction(m_gameContext);
        }

        private IGameContext m_gameContext;
        private ITurnHandler m_turnHandler;
        private IPlayerAction m_playerAction;
        private IPlayerActionReceiver m_playerActionReceiver;
        private Player m_player;
        private Law m_law;
    }
}
