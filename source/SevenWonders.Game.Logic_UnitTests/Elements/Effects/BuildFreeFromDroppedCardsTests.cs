using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Effects;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Handlers;
using SevenWonders.Game.Logic.PlayerActions;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class BuildFreeFromDroppedCardsTests
    {
        [SetUp]
        public void Setup()
        {
            m_buildFreeFromDroppedCards = new BuildFreeFromDroppedCards();
        }

        [Test]
        public void When_Clone_Called()
        {
            BuildFreeFromDroppedCards buildFreeFromDropped = m_buildFreeFromDroppedCards.Clone();

            Assert.That(buildFreeFromDropped, Is.Not.Null);
            Assert.That(m_buildFreeFromDroppedCards, Is.Not.EqualTo(buildFreeFromDropped));
        }

        [Test]
        public void When_Apply_Called_And_DroppedCardList_Is_Null()
        {
            IGameContext gameContext = Substitute.For<IGameContext>();
            Player player = new Player();
            Player opponent = new Player();
            gameContext.DroppedCardList.Returns(null as ICardList);

            Assert.Throws<InvalidOperationException>(() => m_buildFreeFromDroppedCards.Apply(gameContext, player, opponent));
        }

        [Test]
        public void When_Apply_Called()
        {
            ICardList cardList = Substitute.For<ICardList>();
            IGameContext gameContext = Substitute.For<IGameContext>();
            IPlayerActionHandler playerActionHandler = Substitute.For<IPlayerActionHandler>();
            IEventManager eventManager = Substitute.For<IEventManager>();
            Player player = new Player();
            Player opponent = new Player();
            cardList.Cards.Returns([new BrownCard()]);
            gameContext.DroppedCardList.Returns(cardList);
            gameContext.PlayerActionHandler.Returns(playerActionHandler);
            gameContext.EventManager.Returns(eventManager);

            m_buildFreeFromDroppedCards.Apply(gameContext, player, opponent);

            playerActionHandler.Received(1).HandlePlayerActions(gameContext, player, Arg.Any<ICollection<IPlayerAction>>());
        }

        private BuildFreeFromDroppedCards m_buildFreeFromDroppedCards;
    }
}
