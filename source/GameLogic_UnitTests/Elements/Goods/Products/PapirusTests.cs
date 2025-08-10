using GameLogic.Elements.Goods.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Goods.Products
{
    public class PapirusTests
    {
        [Test]
        public void When_Default_Constructor_Called()
        {
            Papirus papirus = new Papirus();

            Assert.That(papirus, Is.Not.Null);
            Assert.That(papirus.Amount, Is.EqualTo(0));
        }

        [Test]
        public void When_Clone_Called()
        {
            Papirus papirus = new Papirus();
            papirus.Amount = 100;

            Papirus clonedPapirus = papirus.Clone();

            Assert.That(clonedPapirus, Is.Not.Null);
            Assert.That(clonedPapirus, Is.Not.EqualTo(papirus));
            Assert.That(clonedPapirus.Amount, Is.EqualTo(papirus.Amount));
        }
    }
}
