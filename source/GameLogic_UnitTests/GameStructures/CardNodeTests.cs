using GameLogic.Elements.GameCards;
using GameLogic.GameStructures;
using NSubstitute;

namespace GameLogic_UnitTests.GameStructures
{
    public class CardNodeTests
    {
        [SetUp]
        public void Setup()
        {
            m_card = Substitute.For<Card>();
            m_cardNode = new CardNode(m_card);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new CardNode(null));
        }

        [Test]
        public void When_AddParent_Called_With_Itself()
        {
            Assert.Throws<ArgumentException>(() => m_cardNode.AddParent(m_cardNode));
        }

        [Test]
        public void When_AddParent_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => m_cardNode.AddParent(null));
        }

        [Test]
        public void When_AddParent_Called_And_2_Parent_Is_Already_There()
        {
            Assert.DoesNotThrow(() => m_cardNode.AddParent(Substitute.For<ICardNode>()));
            Assert.DoesNotThrow(() => m_cardNode.AddParent(Substitute.For<ICardNode>()));
            Assert.Throws<InvalidOperationException>(() => m_cardNode.AddParent(Substitute.For<ICardNode>()));
        }

        [Test]
        public void When_AddParent_Called_Success()
        {
            ICardNode parent = Substitute.For<ICardNode>();
            Assert.DoesNotThrow(() => m_cardNode.AddParent(parent));
            Assert.That(m_cardNode.CoveredBy.Count == 1, Is.True);
            Assert.That(m_cardNode.CoveredBy.First() == parent, Is.True);
        }

        [Test]
        public void When_RemoveParent_Called_Success()
        {
            ICardNode parent = Substitute.For<ICardNode>();
            Assert.DoesNotThrow(() => m_cardNode.AddParent(parent));
            Assert.That(m_cardNode.CoveredBy.Count == 1, Is.True);
            Assert.That(m_cardNode.CoveredBy.Contains(parent), Is.True);
            Assert.DoesNotThrow(() => m_cardNode.RemoveParent(parent));
            Assert.That(m_cardNode.CoveredBy.Count == 0, Is.True);
            Assert.That(!m_cardNode.CoveredBy.Contains(parent), Is.True);
        }

        [Test]
        public void When_RemoveParent_Called_Without_Parent()
        {
            ICardNode parent = Substitute.For<ICardNode>();
            Assert.DoesNotThrow(() => m_cardNode.RemoveParent(parent));
        }

        private Card m_card;
        private CardNode m_cardNode;
    }
}
