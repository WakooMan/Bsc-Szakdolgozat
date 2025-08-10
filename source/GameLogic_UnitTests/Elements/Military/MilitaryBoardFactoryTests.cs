using GameLogic.Elements.Military;
using NSubstitute;
using SevenWonders.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Military
{
    public class MilitaryBoardFactoryTests
    {
        [SetUp]
        public void Setup()
        {
            m_militaryBoard = new MilitaryBoard();
            m_xmlHandler = Substitute.For<IXmlHandler>();
            m_xmlHandler.Deserialize<MilitaryBoard>(Arg.Any<string>()).Returns(m_militaryBoard);
            m_militaryBoardFactory = new MilitaryBoardFactory(m_xmlHandler);
        }

        [Test]
        public void When_Create_Called()
        {
            IMilitaryBoard militaryBoard = m_militaryBoardFactory.Create();

            Assert.That(militaryBoard, Is.EqualTo(m_militaryBoard));
            m_xmlHandler.Received(1).Deserialize<MilitaryBoard>(Arg.Any<string>());
        }

        private MilitaryBoard m_militaryBoard;
        private IXmlHandler m_xmlHandler;
        private MilitaryBoardFactory m_militaryBoardFactory;
    }
}
