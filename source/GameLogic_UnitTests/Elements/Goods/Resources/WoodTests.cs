using GameLogic.Elements.Goods.Products;
using GameLogic.Elements.Goods.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Goods.Resources
{
    public class WoodTests
    {
        [Test]
        public void When_Default_Constructor_Called()
        {
            Wood wood = new Wood();

            Assert.That(wood, Is.Not.Null);
            Assert.That(wood.Amount, Is.EqualTo(0));
        }

        [Test]
        public void When_Clone_Called()
        {
            Wood wood = new Wood();
            wood.Amount = 100;

            Wood clonedWood = wood.Clone();

            Assert.That(clonedWood, Is.Not.Null);
            Assert.That(clonedWood, Is.Not.EqualTo(wood));
            Assert.That(clonedWood.Amount, Is.EqualTo(wood.Amount));
        }
    }
}
