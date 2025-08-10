using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Handlers;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ITurnHandler turnHandler = Substitute.For<ITurnHandler>();
            IPlayerAction playerAction = Substitute.For<IPlayerAction>();
            IPlayerActionReceiver playerActionReceiver = Substitute.For<IPlayerActionReceiver>();
            Player player = new Player();
            turnHandler.CurrentPlayer.Returns(player);
            playerAction.CanPerform(gameContext).Returns(true);
            playerActionReceiver.ReceivePlayerAction(player, Arg.Any<ICollection<IPlayerAction>>()).Returns(playerAction);
            gameContext.PlayerActionReceiver.Returns(playerActionReceiver);
            gameContext.TurnHandler.Returns(turnHandler);
            gameContext.DroppedCardList.Returns(null as ICardList);

            Assert.Throws<InvalidOperationException>(()=> m_buildFreeFromDroppedCards.Apply(gameContext));

            playerActionReceiver.DidNotReceive().ReceivePlayerAction(player, Arg.Any<ICollection<IPlayerAction>>());
            playerAction.DidNotReceive().CanPerform(gameContext);
            playerAction.DidNotReceive().DoPlayerAction(gameContext);
        }

        [Test]
        public void When_Apply_Called()
        {
            ICardList cardList = Substitute.For<ICardList>();
            IGameContext gameContext = Substitute.For<IGameContext>();
            ITurnHandler turnHandler = Substitute.For<ITurnHandler>();
            IPlayerAction playerAction = Substitute.For<IPlayerAction>();
            IPlayerActionReceiver playerActionReceiver = Substitute.For<IPlayerActionReceiver>();
            Player player = new Player();
            cardList.Cards.Returns([]);
            turnHandler.CurrentPlayer.Returns(player);
            playerAction.CanPerform(gameContext).Returns(true);
            playerActionReceiver.ReceivePlayerAction(player, Arg.Any<ICollection<IPlayerAction>>()).Returns(playerAction);
            gameContext.PlayerActionReceiver.Returns(playerActionReceiver);
            gameContext.TurnHandler.Returns(turnHandler);
            gameContext.DroppedCardList.Returns(cardList);

            m_buildFreeFromDroppedCards.Apply(gameContext);

            playerActionReceiver.Received(1).ReceivePlayerAction(player, Arg.Any<ICollection<IPlayerAction>>());
            playerAction.Received(1).CanPerform(gameContext);
            playerAction.Received(1).DoPlayerAction(gameContext);
        }

        private BuildFreeFromDroppedCards m_buildFreeFromDroppedCards;
    }
}
