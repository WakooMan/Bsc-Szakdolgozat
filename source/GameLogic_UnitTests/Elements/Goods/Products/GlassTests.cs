using GameLogic.Elements.Goods.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Goods.Products
{
    public class GlassTests
    {
        [Test]
        public void When_Default_Constructor_Called()
        {
            Glass glass = new Glass();

            Assert.That(glass, Is.Not.Null);
            Assert.That(glass.Amount, Is.EqualTo(0));
        }

        [Test]
        public void When_Clone_Called()
        {
            Glass glass = new Glass();
            glass.Amount = 100;

            Glass clonedGlass = glass.Clone();

            Assert.That(clonedGlass, Is.Not.Null);
            Assert.That(clonedGlass, Is.Not.EqualTo(glass));
            Assert.That(clonedGlass.Amount, Is.EqualTo(glass.Amount));
        }
    }
}
