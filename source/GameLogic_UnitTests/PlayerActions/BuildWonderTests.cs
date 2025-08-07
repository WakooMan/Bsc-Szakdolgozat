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
    public class BuildWonderTests
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
            Wonder wonder = new Wonder() { Name = "Wonder1", HasBeenBuilt = false };
            m_current.Wonders.Add(wonder);
            BuildWonder buildWonder = new BuildWonder(wonder);

            Assert.That(buildWonder.CanPerform(m_gameContext), Is.False);
        }

        [Test]
        public void When_CanPerform_Called_And_Player_Does_Not_Have_Wonder()
        {
            m_current.PickedCard = new CardNode(new RedCard() { Name = "Building2", PreviousBuilding = "Building1" });
            Wonder wonder = new Wonder() { Name = "Wonder1", HasBeenBuilt = false };
            BuildWonder buildWonder = new BuildWonder(wonder);

            Assert.That(buildWonder.CanPerform(m_gameContext), Is.False);
        }

        [Test]
        public void When_CanPerform_Called_And_HasBeenBuilt_Is_True()
        {
            m_current.PickedCard = new CardNode(new RedCard() { Name = "Building2", PreviousBuilding = "Building1" });
            Wonder wonder = new Wonder() { Name = "Wonder1", HasBeenBuilt = true };
            m_current.Wonders.Add(wonder);
            BuildWonder buildWonder = new BuildWonder(wonder);

            Assert.That(buildWonder.CanPerform(m_gameContext), Is.False);
        }

        [Test]
        public void When_CanPerform_Called_And_CanAfford_Returns_True()
        {
            m_current.PickedCard = new CardNode(new RedCard() { Name = "Building2", PreviousBuilding = "Building1" });
            Wonder wonder = new Wonder() { Name = "Wonder1", HasBeenBuilt = false };
            m_current.Wonders.Add(wonder);
            m_costCalculator.CanAfford(Arg.Any<IBuildable>(), Arg.Any<Player>(), Arg.Any<Player>()).Returns(true);
            BuildWonder buildWonder = new BuildWonder(wonder);

            Assert.That(buildWonder.CanPerform(m_gameContext), Is.True);
            m_costCalculator.Received(1).CanAfford(Arg.Any<IBuildable>(), Arg.Any<Player>(), Arg.Any<Player>());
        }

        [Test]
        public void When_CanPerform_Called_And_CanAfford_Returns_False()
        {
            m_current.PickedCard = new CardNode(new RedCard() { Name = "Building2", PreviousBuilding = "Building1" });
            Wonder wonder = new Wonder() { Name = "Wonder1", HasBeenBuilt = false };
            m_current.Wonders.Add(wonder);
            m_costCalculator.CanAfford(Arg.Any<IBuildable>(), Arg.Any<Player>(), Arg.Any<Player>()).Returns(false);
            BuildWonder buildWonder = new BuildWonder(wonder);

            Assert.That(buildWonder.CanPerform(m_gameContext), Is.False);
            m_costCalculator.Received(1).CanAfford(Arg.Any<IBuildable>(), Arg.Any<Player>(), Arg.Any<Player>());
        }


        [Test]
        public void When_DoPlayerAction_Called_And_PickedCard_Is_Null()
        {
            Wonder wonder = new Wonder() { Name = "Wonder1", HasBeenBuilt = false };
            m_current.Wonders.Add(wonder);
            BuildWonder buildWonder = new BuildWonder(wonder);
            Assert.Throws<InvalidOperationException>(() => buildWonder.DoPlayerAction(m_gameContext));
        }

        [Test]
        public void When_DoPlayerAction_Called_And_Wonder_Is_Built_Already()
        {
            Wonder wonder = new Wonder() { Name = "Wonder1", HasBeenBuilt = true };
            m_current.Wonders.Add(wonder);
            BuildWonder buildWonder = new BuildWonder(wonder);
            Assert.Throws<InvalidOperationException>(() => buildWonder.DoPlayerAction(m_gameContext));
        }

        [Test]
        public void When_DoPlayerAction_Called_And_Player_Does_Not_Have_Wonder()
        {
            Wonder wonder = new Wonder() { Name = "Wonder1", HasBeenBuilt = false };
            BuildWonder buildWonder = new BuildWonder(wonder);
            Assert.Throws<InvalidOperationException>(() => buildWonder.DoPlayerAction(m_gameContext));
        }

        [TestCase(6, 6)]
        [TestCase(6, 7)]
        [TestCase(6, 5)]
        [TestCase(1, 0)]
        public void When_DoPlayerAction_Called_And_PickedCard_Is_Not_Null_And_BuildCost_Calculated(int buildCost, int playerMoney)
        {
            Wonder wonder = new Wonder() { Name = "Wonder1", HasBeenBuilt = false };
            m_current.Wonders.Add(wonder);
            m_current.Money = playerMoney;
            ICardNode cardNode = new CardNode(new RedCard() { Name = "Building2", PreviousBuilding = "Building1" });
            m_current.PickedCard = cardNode;
            m_costCalculator.GetBuildCost(Arg.Any<IBuildable>(), Arg.Any<Player>(), Arg.Any<Player>()).Returns(buildCost);
            BuildWonder buildCard = new BuildWonder(wonder);

            buildCard.DoPlayerAction(m_gameContext);

            m_cardComposition.Received(1).RemoveCard(cardNode);
            Assert.That(m_current.PickedCard, Is.Null);
            Assert.That(m_current.Cards.Contains(cardNode.CardObj), Is.False);
            Assert.That(wonder.HasBeenBuilt, Is.True);
            m_costCalculator.Received(1).GetBuildCost(Arg.Any<IBuildable>(), Arg.Any<Player>(), Arg.Any<Player>());
            m_eventManager.Received(1).Publish(Arg.Any<OnWonderBuilt>());
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
