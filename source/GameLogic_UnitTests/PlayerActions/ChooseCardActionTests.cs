using GameLogic;
using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.GameStructures;
using GameLogic.Handlers;
using GameLogic.PlayerActions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.PlayerActions
{
    public class ChooseCardActionTests
    {
        [SetUp]
        public void Setup()
        {
            m_costCalculator = Substitute.For<ICostCalculator>();
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_gameContext = Substitute.For<IGameContext>();
            m_cardComposition = Substitute.For<ICardComposition>();
            m_eventManager = Substitute.For<IEventManager>();
            m_age = Substitute.For<IAgeBase>();
            m_ageHandler = Substitute.For<IAgeHandler>();
            m_current = new Player() { Name = "Current" };
            m_opponent = new Player() { Name = "Opponent" };
            m_turnHandler.CurrentPlayer.Returns(m_current);
            m_turnHandler.OpponentPlayer.Returns(m_opponent);
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_gameContext.CostCalculator.Returns(m_costCalculator);
            m_gameContext.AgeHandler.Returns(m_ageHandler);
            m_gameContext.EventManager.Returns(m_eventManager);
            m_ageHandler.CurrentAge.Returns(m_age);
            m_age.Composition.Returns(m_cardComposition);
        }

        [Test]
        public void When_CanPerform_Called_DroppedCardList_Is_Null()
        {
            m_gameContext.DroppedCardList.Returns(null as ICardList);
            ChooseCardAction chooseCardAction = new ChooseCardAction(new RedCard());
            Assert.That(chooseCardAction.CanPerform(m_gameContext), Is.False);
        }

        [Test]
        public void When_CanPerform_Called_DroppedCardList_Does_Not_Contain_Card()
        {
            ICardList cardList = Substitute.For<ICardList>();
            cardList.Cards.Returns([]);
            m_gameContext.DroppedCardList.Returns(cardList);
            ChooseCardAction chooseCardAction = new ChooseCardAction(new RedCard());
            Assert.That(chooseCardAction.CanPerform(m_gameContext), Is.False);
        }

        [Test]
        public void When_CanPerform_Called_Returns_True()
        {
            Card card = new RedCard();
            ICardList cardList = Substitute.For<ICardList>();
            cardList.Cards.Returns([card]);
            m_gameContext.DroppedCardList.Returns(cardList);
            ChooseCardAction chooseCardAction = new ChooseCardAction(card);
            Assert.That(chooseCardAction.CanPerform(m_gameContext), Is.True);
        }

        [Test]
        public void When_DoPlayerAction_Called_And_DroppedCardList_Is_Null()
        {
            m_gameContext.DroppedCardList.Returns(null as ICardList);
            ChooseCardAction chooseCardAction = new ChooseCardAction(new RedCard());
            Assert.Throws<InvalidOperationException>(() => chooseCardAction.DoPlayerAction(m_gameContext));
        }

        [Test]
        public void When_DoPlayerAction_Called_And_DroppedCardList_Does_Not_Contain_Card()
        {
            ICardList cardList = Substitute.For<ICardList>();
            cardList.Cards.Returns([]);
            m_gameContext.DroppedCardList.Returns(cardList);
            ChooseCardAction chooseCardAction = new ChooseCardAction(new RedCard());
            Assert.Throws<InvalidOperationException>(() => chooseCardAction.DoPlayerAction(m_gameContext));
        }

        [Test]
        public void When_DoPlayerAction_Called_Successful()
        {
            Card card = new RedCard();
            List<Card> cards = [card];
            ICardList cardList = Substitute.For<ICardList>();
            cardList.Cards.Returns(cards);
            m_gameContext.DroppedCardList.Returns(cardList);
            ChooseCardAction chooseCardAction = new ChooseCardAction(card);

            chooseCardAction.DoPlayerAction(m_gameContext);

            Assert.That(cards.Contains(card), Is.False);
            Assert.That(m_current.Cards.Contains(card), Is.True);
            m_eventManager.Received(1).Publish(Arg.Any<OnCardBuilt>());
        }

        private Player m_current;
        private Player m_opponent;
        private ICostCalculator m_costCalculator;
        private ITurnHandler m_turnHandler;
        private IGameContext m_gameContext;
        private IAgeHandler m_ageHandler;
        private IAgeBase m_age;
        private ICardComposition m_cardComposition;
        private IEventManager m_eventManager;
    }
}
