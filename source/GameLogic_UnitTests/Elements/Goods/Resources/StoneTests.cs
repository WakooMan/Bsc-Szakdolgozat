using GameLogic.Elements.Goods.Products;
using GameLogic.Elements.Goods.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Goods.Resources
{
    public class StoneTests
    {
        [Test]
        public void When_Default_Constructor_Called()
        {
            Stone stone = new Stone();

            Assert.That(stone, Is.Not.Null);
            Assert.That(stone.Amount, Is.EqualTo(0));
        }

        [Test]
        public void When_Clone_Called()
        {
            Stone stone = new Stone();
            stone.Amount = 100;

            Stone clonedStone = stone.Clone();

            Assert.That(clonedStone, Is.Not.Null);
            Assert.That(clonedStone, Is.Not.EqualTo(stone));
            Assert.That(clonedStone.Amount, Is.EqualTo(stone.Amount));
        }
    }
}
