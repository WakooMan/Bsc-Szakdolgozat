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
    public class PickCardTests
    {
        [SetUp]
        public void Setup()
        {
            m_card = Substitute.For<Card>();
            m_cardNode = Substitute.For<ICardNode>();
            m_cardNode.CardObj.Returns(m_card);
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
            m_pickCard = new PickCard(m_current, m_cardNode);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new PickCard(null, m_cardNode));
            Assert.Throws<ArgumentNullException>(() => new PickCard(m_current, null));
        }

        [Test]
        public void When_CanPerform_Called_And_Composition_Does_Not_Contain_Card()
        {
            m_cardComposition.AvailableCards.Returns([]);

            bool result = m_pickCard.CanPerform(m_gameContext);

            Assert.That(result, Is.False);
        }

        [Test]
        public void When_CanPerform_Called_Successfull()
        {
            m_cardComposition.AvailableCards.Returns([m_cardNode]);

            bool result = m_pickCard.CanPerform(m_gameContext);

            Assert.That(result, Is.True);
        }

        [Test]
        public void When_DoPlayerAction_Called_And_Composition_Does_Not_Contain_Card()
        {
            m_cardComposition.AvailableCards.Returns([]);

            Assert.Throws<InvalidOperationException>(() => m_pickCard.DoPlayerAction(m_gameContext));

            Assert.That(m_current.PickedCard, Is.Null);
        }

        [Test]
        public void When_DoPlayerAction_Called_Successful()
        {
            m_cardComposition.AvailableCards.Returns([m_cardNode]);

            m_pickCard.DoPlayerAction(m_gameContext);

            Assert.That(m_current.PickedCard, Is.Not.Null);
            Assert.That(m_current.PickedCard, Is.EqualTo(m_cardNode));
            m_gameContext.EventManager.Received(1).Publish(Arg.Any<OnCardPicked>());
        }

        private ICardNode m_cardNode;
        private Card m_card;
        private PickCard m_pickCard;
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
