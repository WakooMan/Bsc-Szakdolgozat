using GameLogic.Elements.GameCards;
using NSubstitute;
using SevenWonders.Common;

namespace GameLogic_UnitTests.Elements.GameCards
{
    public class MainCardListFactoryTests
    {
        [SetUp]
        public void Setup()
        {
            m_xmlHandler = Substitute.For<IXmlHandler>();
            m_cardList = new MainCardListFactory(m_xmlHandler);
        }

        [Test]
        public void When_Create_Called()
        {
            ICardList cardList = m_cardList.Create();
            m_xmlHandler.Received(1).Deserialize<CardList>(Arg.Any<string>());
        }

        private IXmlHandler m_xmlHandler;
        private MainCardListFactory m_cardList;
    }
}
