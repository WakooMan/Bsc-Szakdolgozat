using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Effects;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.Handlers;
using SevenWonders.Game.Logic.PlayerActions;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class DropEnemyCardTests
    {
        [SetUp]
        public void Setup()
        {
            m_gameContext = Substitute.For<IGameContext>();
            m_playerActionHandler = Substitute.For<IPlayerActionHandler>();
            m_eventManager = Substitute.For<IEventManager>();
            m_player = new Player();
            m_opponent = new Player();
            m_gameContext.PlayerActionHandler.Returns(m_playerActionHandler);
            m_gameContext.EventManager.Returns(m_eventManager);
            m_dropEnemyCard = new DropEnemyCard();
        }

        [Test]
        public void When_Clone_Called()
        {
            DropEnemyCard dropEnemyCard = m_dropEnemyCard.Clone();

            Assert.That(dropEnemyCard, Is.Not.Null);
            Assert.That(m_dropEnemyCard, Is.Not.EqualTo(dropEnemyCard));
        }

        [Test]
        public void When_Apply_Called()
        {
            m_dropEnemyCard.CardType = nameof(RedCard);
            m_opponent.Cards.Add(new RedCard());

            m_dropEnemyCard.Apply(m_gameContext, m_player, m_opponent);

            m_eventManager.Received(1).Publish(Arg.Any<OnChooseObjects>());
            m_playerActionHandler.Received(1).HandlePlayerActions(m_gameContext, m_player, Arg.Any<ICollection<IPlayerAction>>());
        }

        private IGameContext m_gameContext;
        private IPlayerActionHandler m_playerActionHandler;
        private IEventManager m_eventManager;
        private Player m_player;
        private Player m_opponent;
        private DropEnemyCard m_dropEnemyCard;
    }
}
