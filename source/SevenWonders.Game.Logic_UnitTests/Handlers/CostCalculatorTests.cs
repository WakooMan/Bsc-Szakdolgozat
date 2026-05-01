using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Effects;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Elements.Goods.Resources;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Handlers;
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
            IBuildable buildable = Substitute.For<IBuildable>();
            buildable.BuildingType.Returns(nameof(RedCard));
            buildable.MoneyCost.Returns(0);
            buildable.GoodCost.Returns([new Clay() { Amount = 3 }, new Stone() { Amount = 3}, new Wood() { Amount = 3 }]);
            Player player = new Player() { Name = "test", Id = 1 };
            player.Cards.AddRange([new BrownCard() { ProducedResources = [new Clay() { Amount = 2 }, new Stone() { Amount = 2 }, new Wood() { Amount = 2 }] }]);
            player.Cards.Add(new YellowCard() { Effects = [new BuyGoods() { BuyGoodItems = [new BuyGoodItem() { MoneyCost = 1, GoodType = nameof(Clay) }] }] });
            Player opponent = new Player() { Name = "test2", Id = 2 };
            opponent.Cards.AddRange([new BrownCard() { ProducedResources = [new Clay() { Amount = 2 }, new Stone() { Amount = 2 }] }]);
            int cost = m_costCalculator.GetBuildCost(buildable, player, opponent);

            Assert.That(cost, Is.EqualTo(7));
        }

        [Test]
        public void When_GetBuildCost_Called_With_Cheaper_Building_And_Building_Type_Is_Same()
        {
            IBuildable buildable = Substitute.For<IBuildable>();
            buildable.BuildingType.Returns(nameof(RedCard));
            buildable.MoneyCost.Returns(0);
            buildable.GoodCost.Returns([new Clay() { Amount = 3 }, new Stone() { Amount = 3 }, new Wood() { Amount = 3 }]);
            Player player = new Player() { Name = "test", Id = 1 };
            player.Cards.AddRange([new BrownCard() { ProducedResources = [new Clay() { Amount = 2 }, new Stone() { Amount = 2 }, new Wood() { Amount = 2 }] }]);
            player.Cards.Add(new YellowCard() { Effects = [new CheaperBuilding() { AmountOfResources = 2, BuildingType = nameof(RedCard) }] });
            Player opponent = new Player() { Name = "test2", Id = 2 };
            int cost = m_costCalculator.GetBuildCost(buildable, player, opponent);

            Assert.That(cost, Is.EqualTo(2));
        }

        [Test]
        public void When_GetBuildCost_Called_With_Cheaper_Building_And_Building_Type_Is_Not_Same()
        {
            IBuildable buildable = Substitute.For<IBuildable>();
            buildable.BuildingType.Returns(nameof(RedCard));
            buildable.MoneyCost.Returns(0);
            buildable.GoodCost.Returns([new Clay() { Amount = 3 }, new Stone() { Amount = 3 }, new Wood() { Amount = 3 }]);
            Player player = new Player() { Name = "test", Id = 1 };
            player.Cards.AddRange([new BrownCard() { ProducedResources = [new Clay() { Amount = 2 }, new Stone() { Amount = 2 }, new Wood() { Amount = 2 }] }]);
            player.Cards.Add(new YellowCard() { Effects = [new CheaperBuilding() { AmountOfResources = 2, BuildingType = nameof(YellowCard) }] });
            Player opponent = new Player() { Name = "test2", Id = 2 };
            int cost = m_costCalculator.GetBuildCost(buildable, player, opponent);

            Assert.That(cost, Is.EqualTo(6));
        }

        [Test]
        public void When_GetBuildCost_Called_And_Player_has_Enough_Resources()
        {
            IBuildable buildable = Substitute.For<IBuildable>();
            buildable.BuildingType.Returns(nameof(RedCard));
            buildable.MoneyCost.Returns(0);
            buildable.GoodCost.Returns([new Clay() { Amount = 3 }, new Stone() { Amount = 3 }, new Wood() { Amount = 3 }]);
            Player player = new Player() { Name = "test", Id = 1 };
            player.Cards.AddRange([new BrownCard() { ProducedResources = [new Clay() { Amount = 3 }, new Stone() { Amount = 3 }, new Wood() { Amount = 3 }] }]);
            Player opponent = new Player() { Name = "test2", Id = 2 };
            int cost = m_costCalculator.GetBuildCost(buildable, player, opponent);

            Assert.That(cost, Is.EqualTo(0));
        }

        [Test]
        public void When_GetBuildCost_Called_And_Player_has_Enough_Resources_But_Buildable_Has_MoneyCost()
        {
            IBuildable buildable = Substitute.For<IBuildable>();
            buildable.BuildingType.Returns(nameof(RedCard));
            buildable.MoneyCost.Returns(5);
            buildable.GoodCost.Returns([new Clay() { Amount = 3 }, new Stone() { Amount = 3 }, new Wood() { Amount = 3 }]);
            Player player = new Player() { Name = "test", Id = 1 };
            player.Cards.AddRange([new BrownCard() { ProducedResources = [new Clay() { Amount = 3 }, new Stone() { Amount = 3 }, new Wood() { Amount = 3 }] }]);
            Player opponent = new Player() { Name = "test2", Id = 2 };
            int cost = m_costCalculator.GetBuildCost(buildable, player, opponent);

            Assert.That(cost, Is.EqualTo(5));
        }

        [Test]
        public void When_GetBuildCost_Called_And_Player_has_More_Resources()
        {
            IBuildable buildable = Substitute.For<IBuildable>();
            buildable.BuildingType.Returns(nameof(RedCard));
            buildable.MoneyCost.Returns(0);
            buildable.GoodCost.Returns([new Clay() { Amount = 3 }, new Stone() { Amount = 3 }, new Wood() { Amount = 3 }]);
            Player player = new Player() { Name = "test", Id = 1 };
            player.Cards.AddRange([new BrownCard() { ProducedResources = [new Clay() { Amount = 4 }, new Stone() { Amount = 4 }, new Wood() { Amount = 4 }] }]);
            Player opponent = new Player() { Name = "test2", Id = 2 };
            int cost = m_costCalculator.GetBuildCost(buildable, player, opponent);

            Assert.That(cost, Is.EqualTo(0));
        }

        [Test]
        public void When_CanAfford_Called_And_Player_Can_Afford()
        {
            IBuildable buildable = Substitute.For<IBuildable>();
            buildable.BuildingType.Returns(nameof(RedCard));
            buildable.MoneyCost.Returns(0);
            buildable.GoodCost.Returns([new Clay() { Amount = 3 }, new Stone() { Amount = 3 }, new Wood() { Amount = 3 }]);
            Player player = new Player() { Name = "test", Id = 1 };
            player.Money = 6;
            player.Cards.AddRange([new BrownCard() { ProducedResources = [new Clay() { Amount = 2 }, new Stone() { Amount = 2 }, new Wood() { Amount = 2 }] }]);
            Player opponent = new Player() { Name = "test2", Id = 2 };
            bool result = m_costCalculator.CanAfford(buildable, player, opponent);

            Assert.That(result, Is.True);
        }

        [Test]
        public void When_CanAfford_Called_And_Player_Cannot_Afford()
        {
            IBuildable buildable = Substitute.For<IBuildable>();
            buildable.BuildingType.Returns(nameof(RedCard));
            buildable.MoneyCost.Returns(0);
            buildable.GoodCost.Returns([new Clay() { Amount = 3 }, new Stone() { Amount = 3 }, new Wood() { Amount = 3 }]);
            Player player = new Player() { Name = "test", Id = 1 };
            player.Money = 5;
            player.Cards.AddRange([new BrownCard() { ProducedResources = [new Clay() { Amount = 2 }, new Stone() { Amount = 2 }, new Wood() { Amount = 2 }] }]);
            Player opponent = new Player() { Name = "test2", Id = 2 };
            bool result = m_costCalculator.CanAfford(buildable, player, opponent);

            Assert.That(result, Is.False);
        }

        private CostCalculator m_costCalculator;
        private IEventManager m_eventManager;
    }
}
