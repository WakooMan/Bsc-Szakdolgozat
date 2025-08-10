using GameLogic.Elements.Goods.Products;
using GameLogic.Elements.Goods.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Goods.Resources
{
    public class ClayTests
    {
        [Test]
        public void When_Default_Constructor_Called()
        {
            Clay clay = new Clay();

            Assert.That(clay, Is.Not.Null);
            Assert.That(clay.Amount, Is.EqualTo(0));
        }

        [Test]
        public void When_Clone_Called()
        {
            Clay clay = new Clay();
            clay.Amount = 100;

            Clay clonedClay = clay.Clone();

            Assert.That(clonedClay, Is.Not.Null);
            Assert.That(clonedClay, Is.Not.EqualTo(clay));
            Assert.That(clonedClay.Amount, Is.EqualTo(clay.Amount));
        }
    }
}
