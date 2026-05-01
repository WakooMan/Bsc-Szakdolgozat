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
    public class GlassFactoryTests
    {
        [Test]
        public void When_Create_Called()
        {
            GlassFactory glassFactory = new GlassFactory();

            Good good = glassFactory.CreateGood();

            Assert.That(good is Glass, Is.True);
            Assert.That(glassFactory.GoodType, Is.EqualTo(typeof(Glass)));
        }
    }
}
