using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Wonders;
using SevenWonders.Game.Logic.Handlers;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.Game.Logic.PlayerActions;
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
            m_playerActionHandler = Substitute.For<IPlayerActionHandler>();
            m_gameContext.PlayerActionHandler.Returns(m_playerActionHandler);
            m_chooseWonderHandler = new ChooseWonderHandler();
            m_player1 = new Player(null!, "test1", 1, 1);
            m_player2 = new Player(null!, "test2", 2, 1);
            List<Wonder> wonders = new List<Wonder>();
            for (int i = 0; i < 8; i++)
            {
                wonders.Add(new Wonder() { Name = $"testWonder{i}"});
            }
            m_chooseWonderHandler.Initialize([m_player1, m_player2], wonders, m_gameContext);
        }

        [Test]
        public void When_Initialize_Called_With_Not_Exactly_8_Wonders_Or_Not_Exactly_2_Players()
        {
            Assert.Throws<InvalidOperationException>(() => m_chooseWonderHandler.Initialize([new Player(), new Player()], [], m_gameContext));
            List<Wonder> wonders = new List<Wonder>();
            for (int i = 0; i < 8; i++)
            {
                wonders.Add(new Wonder() { Name = $"testWonder{i}" });
            }
            Assert.Throws<InvalidOperationException>(() => m_chooseWonderHandler.Initialize([new Player()], wonders, m_gameContext));
        }

        [Test]
        public void When_ChooseWonder_Called_Once()
        {
            m_playerActionHandler.HandlePlayerActions(m_gameContext, Arg.Any<Player>(), Arg.Any<ICollection<IPlayerAction>>()).Returns((args) =>
            {
                Player player = (Player)args[1];
                player.Wonders.Add(new Wonder());
                return (true, ((ICollection<IPlayerAction>)args[2]).First());
            });

            for (int i = 0; i < 1; i++)
            {
                m_chooseWonderHandler.ChooseWonder();
            }

            m_playerActionHandler.Received(1).HandlePlayerActions(m_gameContext, Arg.Any<Player>(), Arg.Any<ICollection<IPlayerAction>>());
            Assert.That(m_player1.Wonders.Count, Is.EqualTo(1));
            Assert.That(m_player2.Wonders.Count, Is.EqualTo(0));

        }

        [Test]
        public void When_ChooseWonder_Called_Twice()
        {
            m_playerActionHandler.HandlePlayerActions(m_gameContext, Arg.Any<Player>(), Arg.Any<ICollection<IPlayerAction>>()).Returns((args) =>
            {
                Player player = (Player)args[1];
                player.Wonders.Add(new Wonder());
                return (true, ((ICollection<IPlayerAction>)args[2]).First());
            });

            for (int i = 0; i < 2; i++)
            {
                m_chooseWonderHandler.ChooseWonder();
            }

            m_playerActionHandler.Received(2).HandlePlayerActions(m_gameContext, Arg.Any<Player>(), Arg.Any<ICollection<IPlayerAction>>());
            Assert.That(m_player1.Wonders.Count, Is.EqualTo(1));
            Assert.That(m_player2.Wonders.Count, Is.EqualTo(1));
        }

        [Test]
        public void When_ChooseWonder_Called_Eight_Times()
        {
            m_playerActionHandler.HandlePlayerActions(m_gameContext, Arg.Any<Player>(), Arg.Any<ICollection<IPlayerAction>>()).Returns((args) =>
            {
                Player player = (Player)args[1];
                player.Wonders.Add(new Wonder());
                return (true, ((ICollection<IPlayerAction>)args[2]).First());
            });
            m_playerActionHandler.HandlePlayerAction(m_gameContext, Arg.Any<Player>(), Arg.Any<IPlayerAction>()).Returns((args) =>
            {
                Player player = (Player)args[1];
                player.Wonders.Add(new Wonder());
                return true;
            });

            for (int i = 0; i < 8; i++)
            {
                m_chooseWonderHandler.ChooseWonder();
            }

            m_playerActionHandler.Received(6).HandlePlayerActions(m_gameContext, Arg.Any<Player>(), Arg.Any<ICollection<IPlayerAction>>());
            m_playerActionHandler.Received(2).HandlePlayerAction(m_gameContext, Arg.Any<Player>(), Arg.Any<IPlayerAction>());
            Assert.That(m_player1.Wonders.Count, Is.EqualTo(4));
            Assert.That(m_player2.Wonders.Count, Is.EqualTo(4));
        }

        [Test]
        public void When_ChooseWonder_Called_Nine_Times()
        {
            m_playerActionHandler.HandlePlayerActions(m_gameContext, Arg.Any<Player>(), Arg.Any<ICollection<IPlayerAction>>()).Returns((args) =>
            {
                Player player = (Player)args[1];
                player.Wonders.Add(new Wonder());
                return (true, ((ICollection<IPlayerAction>)args[2]).First());
            });
            m_playerActionHandler.HandlePlayerAction(m_gameContext, Arg.Any<Player>(), Arg.Any<IPlayerAction>()).Returns((args) =>
            {
                Player player = (Player)args[1];
                player.Wonders.Add(new Wonder());
                return true;
            });

            for (int i = 0; i < 8; i++)
            {
                m_chooseWonderHandler.ChooseWonder();
            }

            m_playerActionHandler.Received(6).HandlePlayerActions(m_gameContext, Arg.Any<Player>(), Arg.Any<ICollection<IPlayerAction>>());
            m_playerActionHandler.Received(2).HandlePlayerAction(m_gameContext, Arg.Any<Player>(), Arg.Any<IPlayerAction>());
            Assert.That(m_player1.Wonders.Count, Is.EqualTo(4));
            Assert.That(m_player2.Wonders.Count, Is.EqualTo(4));

            Assert.Throws<InvalidOperationException>(m_chooseWonderHandler.ChooseWonder);
        }



        private ChooseWonderHandler m_chooseWonderHandler;
        private IGameContext m_gameContext;
        private IPlayerActionHandler m_playerActionHandler;
        private Player m_player1;
        private Player m_player2;
    }
}
