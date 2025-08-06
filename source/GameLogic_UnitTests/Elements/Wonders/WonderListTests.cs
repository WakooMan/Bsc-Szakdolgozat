using GameLogic.Elements.Wonders;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Wonders
{
    public class WonderListTests
    {
        [SetUp]
        public void Setup()
        {
            m_wonder = Substitute.For<Wonder>();
            m_wonderList = new WonderList()
            {
                Wonders = [m_wonder]
            };
        }

        [Test]
        public void When_Initialized_With_Default_Constructor()
        {
            m_wonderList = new WonderList();

            Assert.That(m_wonderList, Is.Not.Null);
            Assert.That(m_wonderList.Wonders, Is.Not.Null);
            Assert.That(m_wonderList.Wonders.Count, Is.EqualTo(0));
        }

        [Test]
        public void When_Clone_Called()
        {
            IWonderList clonedWonderList = m_wonderList.Clone();

            Assert.That(clonedWonderList, Is.Not.Null);
            Assert.That(clonedWonderList.Equals(m_wonderList), Is.False);
            Assert.That(clonedWonderList.GetType(), Is.EqualTo(m_wonderList.GetType()));

            Assert.That(clonedWonderList.Wonders, Is.Not.Null);
            Assert.That(clonedWonderList.Wonders.Equals(m_wonderList.Wonders), Is.False);
            Assert.That(clonedWonderList.Wonders.Count, Is.EqualTo(m_wonderList.Wonders.Count));
            m_wonder.Received(1).Clone();
        }


        private WonderList m_wonderList;
        private Wonder m_wonder;
    }
}
