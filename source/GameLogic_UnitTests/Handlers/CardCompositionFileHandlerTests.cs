using GameLogic.GameStructures;
using GameLogic.Handlers;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace GameLogic_UnitTests.Handlers
{
    public class CardCompositionFileHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            m_cardNode1 = Substitute.For<ICardNode>();
            m_cardNode2 = Substitute.For<ICardNode>();
            m_cardNode3 = Substitute.For<ICardNode>();
            m_compositionFile = Path.Combine(Path.GetTempPath(),"TestData.csv");
            m_cardCompositionFileHandler = new CardCompositionFileHandler(m_compositionFile);
        }

        [Test]
        public void When_Constructor_Called_With_Null_Or_Empty_String()
        {
            Assert.Throws<ArgumentNullException>(() => new CardCompositionFileHandler(null));
            Assert.Throws<ArgumentNullException>(() => new CardCompositionFileHandler(""));
        }

        [Test]
        public void When_SetCompositionForCards_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => m_cardCompositionFileHandler.SetCompositionForCards(null));
        }

        [Test]
        public void When_SetCompositionForCards_Called_With_Not_Equal_CardNodes_And_File_Lines()
        {
            string[] lines = new string[] { "False;2", "False;2", "True;" };
            List<ICardNode> cardNodes = new List<ICardNode>();
            File.WriteAllLines(m_compositionFile, lines);
            Assert.Throws<InvalidOperationException>(() => m_cardCompositionFileHandler.SetCompositionForCards(cardNodes));
        }

        [Test]
        public void When_SetCompositionForCards_Called_With_Equal_CardNodes_And_File_Lines()
        {
            string[] lines = new string[] { "False;2", "False;2", "True;" };
            List<ICardNode> cardNodes = new List<ICardNode>();
            cardNodes.Add(m_cardNode1);
            cardNodes.Add(m_cardNode2);
            cardNodes.Add(m_cardNode3);
            File.WriteAllLines(m_compositionFile, lines);
            Assert.DoesNotThrow(() => m_cardCompositionFileHandler.SetCompositionForCards(cardNodes));

            m_cardNode1.Received(1).Hidden = false;
            m_cardNode1.Received(1).AddParent(m_cardNode3);
            m_cardNode2.Received(1).Hidden = false;
            m_cardNode2.Received(1).AddParent(m_cardNode3);
            m_cardNode3.Received(1).Hidden = true;
            m_cardNode3.DidNotReceive().AddParent(Arg.Any<ICardNode>());
        }

        private CardCompositionFileHandler m_cardCompositionFileHandler;
        private string m_compositionFile;
        private ICardNode m_cardNode1;
        private ICardNode m_cardNode2;
        private ICardNode m_cardNode3;
    }
}
