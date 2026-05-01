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
