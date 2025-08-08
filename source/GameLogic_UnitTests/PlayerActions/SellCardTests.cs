using GameLogic;
using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.GameStructures;
using GameLogic.Handlers;
using GameLogic.PlayerActions;
using NSubstitute;

namespace GameLogic_UnitTests.PlayerActions
{
    public class SellCardTests
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
            m_sellCard = new SellCard(m_current);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new SellCard(null));
        }

        [Test]
        public void When_CanPerform_Called_And_PickedCard_Is_Null()
        {
            bool result = m_sellCard.CanPerform(m_gameContext);

            Assert.That(result, Is.False);
        }

        [Test]
        public void When_CanPerform_Called_Successfull()
        {
            m_current.PickedCard = Substitute.For<ICardNode>();

            bool result = m_sellCard.CanPerform(m_gameContext);

            Assert.That(result, Is.True);
        }

        [Test]
        public void When_DoPlayerAction_Called_And_PickedCard_Is_Null()
        {
            Assert.Throws<InvalidOperationException>(() => m_sellCard.DoPlayerAction(m_gameContext));
        }

        [Test]
        public void When_DoPlayerAction_Called_Successful()
        {
            ICardNode cardNode = Substitute.For<ICardNode>();
            m_current.PickedCard = cardNode;

            m_sellCard.DoPlayerAction(m_gameContext);

            m_cardComposition.Received(1).RemoveCard(cardNode);
            Assert.That(m_current.PickedCard, Is.Null);
            Assert.That(m_current.Money, Is.EqualTo(2));
            m_gameContext.EventManager.Received(1).Publish(Arg.Any<OnCardSold>());
        }

        [Test]
        public void When_DoPlayerAction_Called_Successful_With_Yellow_Cards()
        {
            m_current.Cards.AddRange([new YellowCard(), new RedCard(), new YellowCard(), new GreenCard()]);
            ICardNode cardNode = Substitute.For<ICardNode>();
            m_current.PickedCard = cardNode;

            m_sellCard.DoPlayerAction(m_gameContext);

            m_cardComposition.Received(1).RemoveCard(cardNode);
            Assert.That(m_current.PickedCard, Is.Null);
            Assert.That(m_current.Money, Is.EqualTo(4));
            m_gameContext.EventManager.Received(1).Publish(Arg.Any<OnCardSold>());
        }

        private SellCard m_sellCard;
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
