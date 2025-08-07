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
    public class BuildCardTests
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
        public void When_CanPerform_Called_And_PickedCard_Is_Null()
        {
            BuildCard buildCard = new BuildCard();
            Assert.That(buildCard.CanPerform(m_gameContext), Is.False);
        }

        [Test]
        public void When_CanPerform_Called_And_PickedCard_Is_Not_Null_And_Previous_Building_Available()
        {
            m_current.PickedCard = new CardNode(new RedCard() { Name= "Building2", PreviousBuilding = "Building1" });
            m_current.Cards.Add(new RedCard() { Name = "Building1" });
            BuildCard buildCard = new BuildCard();

            bool result = buildCard.CanPerform(m_gameContext);

            m_costCalculator.DidNotReceive().CanAfford(Arg.Any<IBuildable>(), Arg.Any<Player>(), Arg.Any<Player>());
            Assert.That(result, Is.True);
        }

        [Test]
        public void When_CanPerform_Called_And_PickedCard_Is_Not_Null_And_CanAfford_Returns_True()
        {
            m_current.PickedCard = new CardNode(new RedCard() { Name = "Building2", PreviousBuilding = "Building1" });
            m_costCalculator.CanAfford(Arg.Any<IBuildable>(), Arg.Any<Player>(), Arg.Any<Player>()).Returns(true);
            BuildCard buildCard = new BuildCard();

            bool result = buildCard.CanPerform(m_gameContext);

            m_costCalculator.Received(1).CanAfford(Arg.Any<IBuildable>(), Arg.Any<Player>(), Arg.Any<Player>());
            Assert.That(result, Is.True);
        }

        [Test]
        public void When_CanPerform_Called_And_PickedCard_Is_Not_Null_And_CanAfford_Returns_False()
        {
            m_current.PickedCard = new CardNode(new RedCard() { Name = "Building2", PreviousBuilding = "Building1" });
            m_costCalculator.CanAfford(Arg.Any<IBuildable>(), Arg.Any<Player>(), Arg.Any<Player>()).Returns(false);
            BuildCard buildCard = new BuildCard();

            bool result = buildCard.CanPerform(m_gameContext);

            m_costCalculator.Received(1).CanAfford(Arg.Any<IBuildable>(), Arg.Any<Player>(), Arg.Any<Player>());
            Assert.That(result, Is.False);
        }

        [Test]
        public void When_DoPlayerAction_Called_And_PickedCard_Is_Null()
        {
            BuildCard buildCard = new BuildCard();
            Assert.Throws<InvalidOperationException>(() => buildCard.DoPlayerAction(m_gameContext));
        }

        [Test]
        public void When_DoPlayerAction_Called_And_PickedCard_Is_Not_Null_And_Previous_Building_Available()
        {
            ICardNode cardNode = new CardNode(new RedCard() { Name = "Building2", PreviousBuilding = "Building1" });
            m_current.PickedCard = cardNode;
            m_current.Cards.Add(new RedCard() { Name = "Building1" });
            BuildCard buildCard = new BuildCard();

            buildCard.DoPlayerAction(m_gameContext);

            m_cardComposition.Received(1).RemoveCard(cardNode);
            Assert.That(m_current.PickedCard, Is.Null);
            Assert.That(m_current.Cards.Contains(cardNode.CardObj));
            m_eventManager.Received(1).Publish(Arg.Any<OnCardBuilt>());

        }

        [TestCase(6, 6)]
        [TestCase(6, 7)]
        [TestCase(6, 5)]
        [TestCase(1, 0)]
        public void When_DoPlayerAction_Called_And_PickedCard_Is_Not_Null_And_BuildCost_Calculated(int buildCost, int playerMoney)
        {
            m_current.Money = playerMoney;
            ICardNode cardNode = new CardNode(new RedCard() { Name = "Building2", PreviousBuilding = "Building1" });
            m_current.PickedCard = cardNode;
            m_costCalculator.GetBuildCost(Arg.Any<IBuildable>(), Arg.Any<Player>(), Arg.Any<Player>()).Returns(buildCost);
            BuildCard buildCard = new BuildCard();

            buildCard.DoPlayerAction(m_gameContext);

            m_cardComposition.Received(1).RemoveCard(cardNode);
            Assert.That(m_current.PickedCard, Is.Null);
            Assert.That(m_current.Cards.Contains(cardNode.CardObj), Is.True);
            m_costCalculator.Received(1).GetBuildCost(Arg.Any<IBuildable>(), Arg.Any<Player>(), Arg.Any<Player>());
            m_eventManager.Received(1).Publish(Arg.Any<OnCardBuilt>());
            Assert.That(m_current.Money, Is.EqualTo(Math.Max(0, playerMoney - buildCost)));
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
