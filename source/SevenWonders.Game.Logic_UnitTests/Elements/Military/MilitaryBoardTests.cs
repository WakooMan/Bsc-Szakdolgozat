using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Elements.Military;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.Handlers;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.Game.Logic.PlayerActions;
using NSubstitute;

namespace GameLogic_UnitTests.Elements.Military
{
    public class MilitaryBoardTests
    {
        [SetUp]
        public void Setup()
        {
            m_gameContext = Substitute.For<IGameContext>();
            m_eventManager = Substitute.For<IEventManager>();
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_player1 = new Player();
            m_player2 = new Player();
            m_turnHandler.CurrentPlayer.Returns(m_player1);
            m_turnHandler.OpponentPlayer.Returns(m_player2);
            m_turnHandler.GetPlayer(1).Returns(m_player1);
            m_turnHandler.GetPlayer(2).Returns(m_player2);
            m_gameContext.EventManager.Returns(m_eventManager);
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_militaryBoard = new MilitaryBoard();
            m_militaryBoard.MilitaryCards.AddRange([new MilitaryCard() { IndexStart = 6, IndexEnd = 9, OwnerId = 1, OpponentId = 2}, new MilitaryCard() { IndexStart = 1, IndexEnd = 4 , OwnerId = 2, OpponentId = 1}]);
            m_militaryBoard.Fields.AddRange([MilitaryField.None, MilitaryField.None, MilitaryField.None, MilitaryField.None, MilitaryField.None, MilitaryField.Shield, MilitaryField.None, MilitaryField.None, MilitaryField.None, MilitaryField.None, MilitaryField.None]);
        }

        [Test]
        public void When_Initialize_Called()
        {
            m_militaryBoard.Initialize([m_player1, m_player2], []);

            Assert.That(m_militaryBoard.Fields[5] == MilitaryField.Shield, Is.True);
        }

        [Test]
        public void When_OnUpdate_Called_Player1_Advances()
        {
            m_militaryBoard.Initialize([m_player1, m_player2], []);

            PlayerProperties player1Props = new PlayerProperties(m_player1, m_player2);
            player1Props.Strength = 10;
            PlayerProperties player2Props = new PlayerProperties(m_player2, m_player1);
            player2Props.Strength = 0;

            m_militaryBoard.OnUpdate(m_gameContext, player1Props, player2Props);

            Assert.That(m_militaryBoard.Fields[5] == MilitaryField.None, Is.True);
            Assert.That(m_militaryBoard.Fields[10] == MilitaryField.Shield, Is.True);
            m_eventManager.Received(1).Publish(Arg.Any<OnMilitaryTokenReachedThreshold>());
            m_eventManager.Received(1).Publish(Arg.Any<MilitaryVictory>());
        }

        [Test]
        public void When_OnUpdate_Called_Player2_Advances()
        {
            m_militaryBoard.Initialize([m_player1, m_player2], []);

            PlayerProperties player1Props = new PlayerProperties(m_player1, m_player2);
            player1Props.Strength = 0;
            PlayerProperties player2Props = new PlayerProperties(m_player2, m_player1);
            player2Props.Strength = 10;

            m_militaryBoard.OnUpdate(m_gameContext, player1Props, player2Props);

            Assert.That(m_militaryBoard.Fields[5] == MilitaryField.None, Is.True);
            Assert.That(m_militaryBoard.Fields[0] == MilitaryField.Shield, Is.True);
            m_eventManager.Received(1).Publish(Arg.Any<OnMilitaryTokenReachedThreshold>());
            m_eventManager.Received(1).Publish(Arg.Any<MilitaryVictory>());
        }

        private Player m_player1;
        private Player m_player2;
        private IGameContext m_gameContext;
        private IEventManager m_eventManager;
        private ITurnHandler m_turnHandler;
        private MilitaryBoard m_militaryBoard;
    }
}
