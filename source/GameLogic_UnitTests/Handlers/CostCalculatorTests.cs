using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Goods.Resources;
using GameLogic.Events;
using GameLogic.Handlers;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void When_GetBuildCost_Called()
        {
            m_eventManager.When((evt) => evt.Publish(GameEventType.BuildingCostCalculated, Arg.Any<OnBuildingCostCalculated>())).Do((cb) =>
            {
                OnBuildingCostCalculated arg = (OnBuildingCostCalculated)cb.Args()[1];
                arg.BuyGoodItems.AddRange([new BuyGoodItem() { MoneyCost = 1, GoodType = nameof(Clay) }]);
            });
            IBuildable buildable = Substitute.For<IBuildable>();
            buildable.BuildingType.Returns(nameof(RedCard));
            buildable.MoneyCost.Returns(5);
            buildable.GoodCost.Returns([new Clay() { Amount = 3 }, new Stone() { Amount = 3}, new Wood() { Amount = 3 }]);
            Player player = new Player("test");
            player.Cards.AddRange([new BrownCard() { ProducedResources = [new Clay() { Amount = 2 }, new Stone() { Amount = 2 }, new Wood() { Amount = 2 }] }]);
            Player opponent = new Player("test2");
            opponent.Cards.AddRange([new BrownCard() { ProducedResources = [new Clay() { Amount = 2 }, new Stone() { Amount = 2 }] }]);
            int cost = m_costCalculator.GetBuildCost(buildable, player, opponent);

            m_eventManager.Received(1).Publish(GameEventType.BuildingCostCalculated, Arg.Any<OnBuildingCostCalculated>());
            Assert.That(cost, Is.EqualTo(7));
        }

        private CostCalculator m_costCalculator;
        private IEventManager m_eventManager;
    }
}
