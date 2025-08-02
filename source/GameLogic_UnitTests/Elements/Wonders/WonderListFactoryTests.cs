using GameLogic.Elements.Wonders;
using NSubstitute;
using SevenWonders.Common;

namespace GameLogic_UnitTests.Elements.Wonders
{
    public class WonderListFactoryTests
    {
        [SetUp]
        public void Setup()
        {
            m_wonderList = new WonderList();
            m_xmlHandler = Substitute.For<IXmlHandler>();
            m_xmlHandler.Deserialize<WonderList>(Arg.Any<string>()).Returns(m_wonderList);
            m_factory = new WonderListFactory(m_xmlHandler);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new WonderListFactory(null));
        }

        [Test]
        public void When_Create_Called()
        {
            Assert.That(m_factory.Create(), Is.EqualTo(m_wonderList));
            m_xmlHandler.Received(1).Deserialize<WonderList>(Arg.Any<string>());
        }

        private WonderListFactory m_factory;
        private IXmlHandler m_xmlHandler;
        private WonderList m_wonderList;
    }
}
