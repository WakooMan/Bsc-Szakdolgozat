using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Goods.Resources;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.Handlers;
using NSubstitute;

namespace GameLogic_UnitTests.Handlers
{
    public class CostCalculatorTests
    {
        [SetUp]
        public void Setup()
        {
            m_eventManager = Substitute.For<IEventManager>();
            m_costCalculator = new CostCalculator(m_eventManager);
        }

        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new CostCalculator(null));
        }

        [Test]
        public void When_GetBuildCost_Called_With_Discount_And_Without_Discount()
        {
            m_eventManager.When((evt) => evt.Publish(GameEvent.BuildingCostCalculated, Arg.Any<OnBuildingCostCalculated>())).Do((cb) =>
            {
                OnBuildingCostCalculated arg = (OnBuildingCostCalculated)cb.Args()[1];
                arg.BuyGoodItems.AddRange([new BuyGoodItem() { MoneyCost = 1, GoodType = nameof(Clay) }]);
            });
            IBuildable buildable = Substitute.For<IBuildable>();
            buildable.BuildingType.Returns(nameof(RedCard));
            buildable.MoneyCost.Returns(0);
            buildable.GoodCost.Returns([new Clay() { Amount = 3 }, new Stone() { Amount = 3}, new Wood() { Amount = 3 }]);
            Player player = new Player("test");
            player.Cards.AddRange([new BrownCard() { ProducedResources = [new Clay() { Amount = 2 }, new Stone() { Amount = 2 }, new Wood() { Amount = 2 }] }]);
            Player opponent = new Player("test2");
            opponent.Cards.AddRange([new BrownCard() { ProducedResources = [new Clay() { Amount = 2 }, new Stone() { Amount = 2 }] }]);
            int cost = m_costCalculator.GetBuildCost(buildable, player, opponent);

            m_eventManager.Received(1).Publish(GameEvent.BuildingCostCalculated, Arg.Any<OnBuildingCostCalculated>());
            Assert.That(cost, Is.EqualTo(7));
        }

        [Test]
        public void When_GetBuildCost_Called_With_Cheaper_Building_And_Building_Type_Is_Same()
        {
            m_eventManager.When((evt) => evt.Publish(GameEvent.BuildingCostCalculated, Arg.Any<OnBuildingCostCalculated>())).Do((cb) =>
            {
                OnBuildingCostCalculated arg = (OnBuildingCostCalculated)cb.Args()[1];
                arg.CheaperBuildings.Add(new CheaperBuilding() { AmountOfResources = 2, BuildingType = nameof(RedCard) });
            });
            IBuildable buildable = Substitute.For<IBuildable>();
            buildable.BuildingType.Returns(nameof(RedCard));
            buildable.MoneyCost.Returns(0);
            buildable.GoodCost.Returns([new Clay() { Amount = 3 }, new Stone() { Amount = 3 }, new Wood() { Amount = 3 }]);
            Player player = new Player("test");
            player.Cards.AddRange([new BrownCard() { ProducedResources = [new Clay() { Amount = 2 }, new Stone() { Amount = 2 }, new Wood() { Amount = 2 }] }]);
            Player opponent = new Player("test2");
            int cost = m_costCalculator.GetBuildCost(buildable, player, opponent);

            m_eventManager.Received(1).Publish(GameEvent.BuildingCostCalculated, Arg.Any<OnBuildingCostCalculated>());
            Assert.That(cost, Is.EqualTo(2));
        }

        [Test]
        public void When_GetBuildCost_Called_With_Cheaper_Building_And_Building_Type_Is_Not_Same()
        {
            m_eventManager.When((evt) => evt.Publish(GameEvent.BuildingCostCalculated, Arg.Any<OnBuildingCostCalculated>())).Do((cb) =>
            {
                OnBuildingCostCalculated arg = (OnBuildingCostCalculated)cb.Args()[1];
                arg.CheaperBuildings.Add(new CheaperBuilding() { AmountOfResources = 2, BuildingType = nameof(YellowCard) });
            });
            IBuildable buildable = Substitute.For<IBuildable>();
            buildable.BuildingType.Returns(nameof(RedCard));
            buildable.MoneyCost.Returns(0);
            buildable.GoodCost.Returns([new Clay() { Amount = 3 }, new Stone() { Amount = 3 }, new Wood() { Amount = 3 }]);
            Player player = new Player("test");
            player.Cards.AddRange([new BrownCard() { ProducedResources = [new Clay() { Amount = 2 }, new Stone() { Amount = 2 }, new Wood() { Amount = 2 }] }]);
            Player opponent = new Player("test2");
            int cost = m_costCalculator.GetBuildCost(buildable, player, opponent);

            m_eventManager.Received(1).Publish(GameEvent.BuildingCostCalculated, Arg.Any<OnBuildingCostCalculated>());
            Assert.That(cost, Is.EqualTo(6));
        }

        [Test]
        public void When_GetBuildCost_Called_And_Player_has_Enough_Resources()
        {
            IBuildable buildable = Substitute.For<IBuildable>();
            buildable.BuildingType.Returns(nameof(RedCard));
            buildable.MoneyCost.Returns(0);
            buildable.GoodCost.Returns([new Clay() { Amount = 3 }, new Stone() { Amount = 3 }, new Wood() { Amount = 3 }]);
            Player player = new Player("test");
            player.Cards.AddRange([new BrownCard() { ProducedResources = [new Clay() { Amount = 3 }, new Stone() { Amount = 3 }, new Wood() { Amount = 3 }] }]);
            Player opponent = new Player("test2");
            int cost = m_costCalculator.GetBuildCost(buildable, player, opponent);

            m_eventManager.Received(1).Publish(GameEvent.BuildingCostCalculated, Arg.Any<OnBuildingCostCalculated>());
            Assert.That(cost, Is.EqualTo(0));
        }

        [Test]
        public void When_GetBuildCost_Called_And_Player_has_Enough_Resources_But_Buildable_Has_MoneyCost()
        {
            IBuildable buildable = Substitute.For<IBuildable>();
            buildable.BuildingType.Returns(nameof(RedCard));
            buildable.MoneyCost.Returns(5);
            buildable.GoodCost.Returns([new Clay() { Amount = 3 }, new Stone() { Amount = 3 }, new Wood() { Amount = 3 }]);
            Player player = new Player("test");
            player.Cards.AddRange([new BrownCard() { ProducedResources = [new Clay() { Amount = 3 }, new Stone() { Amount = 3 }, new Wood() { Amount = 3 }] }]);
            Player opponent = new Player("test2");
            int cost = m_costCalculator.GetBuildCost(buildable, player, opponent);

            m_eventManager.Received(1).Publish(GameEvent.BuildingCostCalculated, Arg.Any<OnBuildingCostCalculated>());
            Assert.That(cost, Is.EqualTo(5));
        }

        [Test]
        public void When_GetBuildCost_Called_And_Player_has_More_Resources()
        {
            IBuildable buildable = Substitute.For<IBuildable>();
            buildable.BuildingType.Returns(nameof(RedCard));
            buildable.MoneyCost.Returns(0);
            buildable.GoodCost.Returns([new Clay() { Amount = 3 }, new Stone() { Amount = 3 }, new Wood() { Amount = 3 }]);
            Player player = new Player("test");
            player.Cards.AddRange([new BrownCard() { ProducedResources = [new Clay() { Amount = 4 }, new Stone() { Amount = 4 }, new Wood() { Amount = 4 }] }]);
            Player opponent = new Player("test2");
            int cost = m_costCalculator.GetBuildCost(buildable, player, opponent);

            m_eventManager.Received(1).Publish(GameEvent.BuildingCostCalculated, Arg.Any<OnBuildingCostCalculated>());
            Assert.That(cost, Is.EqualTo(0));
        }

        [Test]
        public void When_CanAfford_Called_And_Player_Can_Afford()
        {
            IBuildable buildable = Substitute.For<IBuildable>();
            buildable.BuildingType.Returns(nameof(RedCard));
            buildable.MoneyCost.Returns(0);
            buildable.GoodCost.Returns([new Clay() { Amount = 3 }, new Stone() { Amount = 3 }, new Wood() { Amount = 3 }]);
            Player player = new Player("test");
            player.Money = 6;
            player.Cards.AddRange([new BrownCard() { ProducedResources = [new Clay() { Amount = 2 }, new Stone() { Amount = 2 }, new Wood() { Amount = 2 }] }]);
            Player opponent = new Player("test2");
            bool result = m_costCalculator.CanAfford(buildable, player, opponent);

            m_eventManager.Received(1).Publish(GameEvent.BuildingCostCalculated, Arg.Any<OnBuildingCostCalculated>());
            Assert.That(result, Is.True);
        }

        [Test]
        public void When_CanAfford_Called_And_Player_Cannot_Afford()
        {
            IBuildable buildable = Substitute.For<IBuildable>();
            buildable.BuildingType.Returns(nameof(RedCard));
            buildable.MoneyCost.Returns(0);
            buildable.GoodCost.Returns([new Clay() { Amount = 3 }, new Stone() { Amount = 3 }, new Wood() { Amount = 3 }]);
            Player player = new Player("test");
            player.Money = 5;
            player.Cards.AddRange([new BrownCard() { ProducedResources = [new Clay() { Amount = 2 }, new Stone() { Amount = 2 }, new Wood() { Amount = 2 }] }]);
            Player opponent = new Player("test2");
            bool result = m_costCalculator.CanAfford(buildable, player, opponent);

            m_eventManager.Received(1).Publish(GameEvent.BuildingCostCalculated, Arg.Any<OnBuildingCostCalculated>());
            Assert.That(result, Is.False);
        }

        private CostCalculator m_costCalculator;
        private IEventManager m_eventManager;
    }
}
