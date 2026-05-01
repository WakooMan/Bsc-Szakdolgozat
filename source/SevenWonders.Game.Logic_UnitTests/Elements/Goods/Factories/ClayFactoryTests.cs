using SevenWonders.Game.Logic.Elements.Goods;
using SevenWonders.Game.Logic.Elements.Goods.Factories;
using SevenWonders.Game.Logic.Elements.Goods.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Goods.Factories
{
    public class ClayFactoryTests
    {
        [Test]
        public void When_Create_Called()
        {
            ClayFactory clayFactory = new ClayFactory();

            Good good = clayFactory.CreateGood();

            Assert.That(good is Clay, Is.True);
            Assert.That(clayFactory.GoodType, Is.EqualTo(typeof(Clay)));
        }
    }
}
