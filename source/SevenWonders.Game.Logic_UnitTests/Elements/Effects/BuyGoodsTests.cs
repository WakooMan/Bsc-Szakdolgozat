using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Effects;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class BuyGoodsTests
    {
        [SetUp]
        public void Setup()
        {
            m_buyGoods = new BuyGoods();
        }

        [Test]
        public void When_Clone_Called()
        {
            BuyGoods buyGoods = m_buyGoods.Clone();

            Assert.That(buyGoods, Is.Not.Null);
            Assert.That(m_buyGoods, Is.Not.EqualTo(buyGoods));
        }

        [Test]
        public void When_BuyGoodItems_Are_Set()
        {
            List<BuyGoodItem> buyGoodItems = [new BuyGoodItem() { GoodType = "Clay", MoneyCost = 2 }, new BuyGoodItem() { GoodType = "Stone", MoneyCost = 1 }];
            m_buyGoods.BuyGoodItems.AddRange(buyGoodItems);

            Assert.That(m_buyGoods.BuyGoodItems.Count, Is.EqualTo(2));
            Assert.That(buyGoodItems.All(m_buyGoods.BuyGoodItems.Contains), Is.True);
        }

        private BuyGoods m_buyGoods;
    }
}
