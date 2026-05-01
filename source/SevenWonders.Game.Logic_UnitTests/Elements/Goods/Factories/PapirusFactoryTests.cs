using SevenWonders.Game.Logic.Elements.Goods;
using SevenWonders.Game.Logic.Elements.Goods.Factories;
using SevenWonders.Game.Logic.Elements.Goods.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Goods.Factories
{
    public class PapirusFactoryTests
    {
        [Test]
        public void When_Create_Called()
        {
            PapirusFactory papirusFactory = new PapirusFactory();

            Good good = papirusFactory.CreateGood();

            Assert.That(good is Papirus, Is.True);
            Assert.That(papirusFactory.GoodType, Is.EqualTo(typeof(Papirus)));
        }
    }
}
