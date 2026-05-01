using SevenWonders.Game.Logic.Elements.Goods;
using SevenWonders.Game.Logic.Elements.Goods.Factories;
using SevenWonders.Game.Logic.Elements.Goods.Products;
using SevenWonders.Game.Logic.Elements.Goods.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Goods.Factories
{
    public class WoodFactoryTests
    {
        [Test]
        public void When_Create_Called()
        {
            WoodFactory woodFactory = new WoodFactory();

            Good good = woodFactory.CreateGood();

            Assert.That(good is Wood, Is.True);
            Assert.That(woodFactory.GoodType, Is.EqualTo(typeof(Wood)));
        }
    }
}
