using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Factories;
using GameLogic.Elements.Goods.Products;
using GameLogic.Elements.Goods.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Goods.Factories
{
    public class StoneFactoryTests
    {
        [Test]
        public void When_Create_Called()
        {
            StoneFactory stoneFactory = new StoneFactory();

            Good good = stoneFactory.CreateGood();

            Assert.That(good is Stone, Is.True);
            Assert.That(stoneFactory.GoodType, Is.EqualTo(typeof(Stone)));
        }
    }
}
