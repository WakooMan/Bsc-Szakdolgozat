using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Wonders;
using GameLogic.Handlers;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using NSubstitute;
using SevenWonders.Common;

namespace GameLogic_UnitTests.Handlers
{
    public class ChooseWonderHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            m_gameContext = Substitute.For<IGameContext>();
            m_playerActionReceiver = Substitute.For<IPlayerActionReceiver>();
            m_gameContext.PlayerActionReceiver.Returns(m_playerActionReceiver);
            m_chooseWonderHandler = new ChooseWonderHandler(m_gameContext);
            m_player1 = new Player("test1");
            m_player2 = new Player("test2");
            List<Wonder> wonders = new List<Wonder>();
            for (int i = 0; i < 8; i++)
            {
                wonders.Add(new Wonder() { Name = $"testWonder{i}"});
            }
            m_chooseWonderHandler.Initialize([m_player1, m_player2], wonders);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new ChooseWonderHandler(null));
        }

        [Test]
        public void When_Initialize_Called_With_Not_Exactly_8_Wonders_Or_Not_Exactly_2_Players()
        {
            Assert.Throws<InvalidOperationException>(() => m_chooseWonderHandler.Initialize([new Player(), new Player()], []));
            List<Wonder> wonders = new List<Wonder>();
            for (int i = 0; i < 8; i++)
            {
                wonders.Add(new Wonder() { Name = $"testWonder{i}" });
            }
            Assert.Throws<InvalidOperationException>(() => m_chooseWonderHandler.Initialize([new Player()], wonders));
        }

        [Test]
        public void When_ChooseWonder_Called_Once()
        {
            m_playerActionReceiver.ReceivePlayerAction(Arg.Any<Player>(), Arg.Any<ICollection<IPlayerAction>>()).Returns((args) =>
            {
                return ((ICollection<IPlayerAction>)args[1]).First();
            });

            for (int i = 0; i < 1; i++)
            {
                m_chooseWonderHandler.ChooseWonder();
            }

            m_playerActionReceiver.Received(1).ReceivePlayerAction(Arg.Any<Player>(), Arg.Any<ICollection<IPlayerAction>>());
            Assert.That(m_player1.Wonders.Count, Is.EqualTo(1));
            Assert.That(m_player2.Wonders.Count, Is.EqualTo(0));

        }

        [Test]
        public void When_ChooseWonder_Called_Twice()
        {
            m_playerActionReceiver.ReceivePlayerAction(Arg.Any<Player>(), Arg.Any<ICollection<IPlayerAction>>()).Returns((args) =>
            {
                return ((ICollection<IPlayerAction>)args[1]).First();
            });

            for (int i = 0; i < 2; i++)
            {
                m_chooseWonderHandler.ChooseWonder();
            }

            m_playerActionReceiver.Received(2).ReceivePlayerAction(Arg.Any<Player>(), Arg.Any<ICollection<IPlayerAction>>());
            Assert.That(m_player1.Wonders.Count, Is.EqualTo(1));
            Assert.That(m_player2.Wonders.Count, Is.EqualTo(1));
        }

        [Test]
        public void When_ChooseWonder_Called_Eight_Times()
        {
            m_playerActionReceiver.ReceivePlayerAction(Arg.Any<Player>(), Arg.Any<ICollection<IPlayerAction>>()).Returns((args) =>
            {
                return ((ICollection<IPlayerAction>)args[1]).First();
            });

            for (int i = 0; i < 8; i++)
            {
                m_chooseWonderHandler.ChooseWonder();
            }

            m_playerActionReceiver.Received(8).ReceivePlayerAction(Arg.Any<Player>(), Arg.Any<ICollection<IPlayerAction>>());
            Assert.That(m_player1.Wonders.Count, Is.EqualTo(4));
            Assert.That(m_player2.Wonders.Count, Is.EqualTo(4));
        }

        [Test]
        public void When_ChooseWonder_Called_Nine_Times()
        {
            m_playerActionReceiver.ReceivePlayerAction(Arg.Any<Player>(), Arg.Any<ICollection<IPlayerAction>>()).Returns((args) =>
            {
                return ((ICollection<IPlayerAction>)args[1]).First();
            });

            for (int i = 0; i < 8; i++)
            {
                m_chooseWonderHandler.ChooseWonder();
            }

            m_playerActionReceiver.Received(8).ReceivePlayerAction(Arg.Any<Player>(), Arg.Any<ICollection<IPlayerAction>>());
            Assert.That(m_player1.Wonders.Count, Is.EqualTo(4));
            Assert.That(m_player2.Wonders.Count, Is.EqualTo(4));

            Assert.Throws<InvalidOperationException>(m_chooseWonderHandler.ChooseWonder);
        }



        private ChooseWonderHandler m_chooseWonderHandler;
        private IGameContext m_gameContext;
        private IPlayerActionReceiver m_playerActionReceiver;
        private Player m_player1;
        private Player m_player2;
    }
}
