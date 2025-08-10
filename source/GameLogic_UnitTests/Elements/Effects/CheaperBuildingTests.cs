using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.Handlers;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class CheaperBuildingTests
    {
        [SetUp]
        public void Setup()
        {
            m_gameContext = Substitute.For<IGameContext>();
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_eventManager = Substitute.For<IEventManager>();
            m_player = new Player();
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_gameContext.EventManager.Returns(m_eventManager);
            m_turnHandler.CurrentPlayer.Returns(m_player);
            m_cheaperBuilding = new CheaperBuilding();
        }

        [Test]
        public void When_Clone_Called()
        {
            m_cheaperBuilding.AmountOfResources = 5;
            m_cheaperBuilding.BuildingType = "test";
            CheaperBuilding cheaperBuilding = m_cheaperBuilding.Clone();

            Assert.That(cheaperBuilding, Is.Not.Null);
            Assert.That(m_cheaperBuilding, Is.Not.EqualTo(cheaperBuilding));
            Assert.That(cheaperBuilding.AmountOfResources, Is.EqualTo(m_cheaperBuilding.AmountOfResources));
            Assert.That(cheaperBuilding.BuildingType, Is.EqualTo(m_cheaperBuilding.BuildingType));
        }

        [Test]
        public void When_Apply_Called()
        {
            m_cheaperBuilding.Apply(m_gameContext);

            Player player = m_turnHandler.Received(1).CurrentPlayer;
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnBuildingCostCalculated>>());
        }

        [Test]
        public void When_OnBuildingCostCalculated_Called()
        {
            OnBuildingCostCalculated onBuildingCostCalculated = new OnBuildingCostCalculated(m_player);
            m_eventManager.When((arg) => arg.Subscribe(Arg.Any<Action<OnBuildingCostCalculated>>())).Do((callinfo) => ((Action<OnBuildingCostCalculated>)callinfo[0])(onBuildingCostCalculated));

            m_cheaperBuilding.Apply(m_gameContext);

            _ = m_turnHandler.Received(1).CurrentPlayer;
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnBuildingCostCalculated>>());
            Assert.That(onBuildingCostCalculated.CheaperBuildings.Contains(m_cheaperBuilding), Is.True);
        }

        private IEventManager m_eventManager;
        private IGameContext m_gameContext;
        private ITurnHandler m_turnHandler;
        private Player m_player;
        private CheaperBuilding m_cheaperBuilding;
    }
}
