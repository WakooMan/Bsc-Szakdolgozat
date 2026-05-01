using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Developments;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Elements.Wonders;
using NSubstitute;

namespace GameLogic_UnitTests.Elements
{
    public class GameElementsTests
    {
        [SetUp]
        public void Setup()
        {
            m_cardListFactory = Substitute.For<ICardListFactory>();
            m_wonderListFactory = Substitute.For<IWonderListFactory>();
            m_developmentListFactory = Substitute.For<IDevelopmentListFactory>();
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new GameElements(null, m_wonderListFactory, m_developmentListFactory));
            Assert.Throws<ArgumentNullException>(() => new GameElements(m_cardListFactory, null, m_developmentListFactory));
            Assert.Throws<ArgumentNullException>(() => new GameElements(m_cardListFactory, m_wonderListFactory, null));
        }

        [Test]
        public void When_Constructor_Called()
        {
            new GameElements(m_cardListFactory, m_wonderListFactory, m_developmentListFactory);
            m_cardListFactory.DidNotReceive().Create();
            m_wonderListFactory.DidNotReceive().Create();
            m_developmentListFactory.DidNotReceive().Create();
        }

        [Test]
        public void When_ResetElements_Called()
        {
            IGameElements gameElements = new GameElements(m_cardListFactory, m_wonderListFactory, m_developmentListFactory);

            gameElements.ResetElements();

            m_cardListFactory.Received(1).Create();
            m_wonderListFactory.Received(1).Create();
            m_developmentListFactory.Received(1).Create();
        }

        private ICardListFactory m_cardListFactory;
        private IWonderListFactory m_wonderListFactory;
        private IDevelopmentListFactory m_developmentListFactory;
    }
}
