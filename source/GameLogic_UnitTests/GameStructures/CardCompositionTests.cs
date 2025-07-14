using GameLogic.Elements.GameCards;
using GameLogic.GameStructures;
using GameLogic.GameStructures.Factories;
using GameLogic.Handlers;
using NSubstitute;

namespace GameLogic_UnitTests.GameStructures
{
    public class CardCompositionTests
    {
        [SetUp]
        public void Setup()
        {
            m_cards = new List<ICard>();
            for (int i = 0; i < 20; i++)
            {
                m_cards.Add(Substitute.For<ICard>());
            }

            m_cardNodeFactory = Substitute.For<ICardNodeFactory>();
            m_cardNodeFactory.Create(Arg.Any<ICard>()).Returns((info) =>
            {
                ICardNode cardNode = Substitute.For<ICardNode>();
                cardNode.CoveredBy.Returns(new List<ICardNode>());
                return cardNode;
            });

            m_cardCompositionFileHandler = Substitute.For<ICardCompositionFileHandler>();
            m_cardCompositionFileHandler.When(handler => handler.SetCompositionForCards(Arg.Any<List<ICardNode>>())).Do(info =>
            {
                List<ICardNode> nodes = info.Arg<List<ICardNode>>();
                m_cardNode1 = nodes[0];
                m_cardNode2 = nodes[1];
                List<ICardNode> coveredList = new List<ICardNode>(new ICardNode[] { m_cardNode2 });
                m_cardNode1.CoveredBy.Returns(_ => coveredList);
            });
            m_cardComposition = new CardComposition(m_cardCompositionFileHandler, m_cardNodeFactory, m_cards);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new CardComposition(null, m_cardNodeFactory, m_cards));
            Assert.Throws<ArgumentNullException>(() => new CardComposition(m_cardCompositionFileHandler, null, m_cards));
            Assert.Throws<ArgumentNullException>(() => new CardComposition(m_cardCompositionFileHandler, m_cardNodeFactory, null));
        }

        [Test]
        public void When_Constructor_Called_With_Empty_List()
        {
            List<ICard> cards = new List<ICard>();
            Assert.Throws<ArgumentException>(() => new CardComposition(m_cardCompositionFileHandler, m_cardNodeFactory, cards));
        }

        [Test]
        public void When_RemoveCard_Called_Success_No_CoveredBy()
        {
            ICardNode cardNode = m_cardComposition.AvailableCards.First();

            m_cardComposition.RemoveCard(cardNode);

            Assert.That(!m_cardComposition.AvailableCards.Contains(cardNode), Is.True);
        }

        [Test]
        public void When_RemoveCard_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => m_cardComposition.RemoveCard(null));
        }

        [Test]
        public void When_RemoveCard_Called_Success_CoveredBy()
        {
            Assert.That(!m_cardComposition.AvailableCards.Contains(m_cardNode1), Is.True);
            Assert.That(m_cardComposition.AvailableCards.Contains(m_cardNode2), Is.True);
            m_cardComposition.RemoveCard(m_cardNode2);
            m_cardNode1.Received(1).RemoveParent(m_cardNode2);
            Assert.That(!m_cardComposition.AvailableCards.Contains(m_cardNode2), Is.True);
        }

        private CardComposition m_cardComposition;
        private ICardCompositionFileHandler m_cardCompositionFileHandler;
        private ICardNodeFactory m_cardNodeFactory;
        private List<ICard> m_cards;
        private ICardNode m_cardNode1;
        private ICardNode m_cardNode2;
    }
}
