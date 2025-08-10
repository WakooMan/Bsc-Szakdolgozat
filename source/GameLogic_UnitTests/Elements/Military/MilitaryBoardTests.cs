using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Disciplines;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Military;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Military
{
    public class MilitaryBoardTests
    {
        [SetUp]
        public void Setup()
        {
            m_gameContext = Substitute.For<IGameContext>();
            m_eventManager = Substitute.For<IEventManager>();
            m_playerActionReceiver = Substitute.For<IPlayerActionReceiver>();
            m_playerAction = Substitute.For<IPlayerAction>();
            m_playerActionReceiver.ReceivePlayerAction(Arg.Any<Player>(), Arg.Any<ICollection<IPlayerAction>>()).Returns(m_playerAction);
            m_player1 = new Player();
            m_player2 = new Player();
            m_gameContext.EventManager.Returns(m_eventManager);
            m_gameContext.PlayerActionReceiver.Returns(m_playerActionReceiver);
            m_militaryBoard = new MilitaryBoard();
            m_militaryBoard.MilitaryCards.AddRange([new MilitaryCard() { IndexStart = 6, IndexEnd = 9}, new MilitaryCard() { IndexStart = 1, IndexEnd = 4 }]);
            m_militaryBoard.Fields.AddRange([MilitaryField.None, MilitaryField.None, MilitaryField.None, MilitaryField.None, MilitaryField.None, MilitaryField.Shield, MilitaryField.None, MilitaryField.None, MilitaryField.None, MilitaryField.None, MilitaryField.None]);
        }

        [Test]
        public void When_Initalize_Called_OnMilitaryAdvanced_Event_Happens_Player1_Advances()
        {
            OnMilitaryAdvanced onMilitaryAdvanced = new OnMilitaryAdvanced(m_player1, 10);
            m_eventManager.When(evt => evt.Subscribe(Arg.Any<Action<OnMilitaryAdvanced>>())).Do(callinfo => ((Action<OnMilitaryAdvanced>)callinfo[0])(onMilitaryAdvanced));

            m_militaryBoard.Initialize([m_player1, m_player2], [], m_gameContext);

            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnMilitaryAdvanced>>());
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnScientificProgress>>());
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnMilitaryTokenReachedThreshold>>());
            Assert.That(m_militaryBoard.Fields[5] == MilitaryField.None, Is.True);
            Assert.That(m_militaryBoard.Fields[10] == MilitaryField.Shield, Is.True);
            m_eventManager.Received(1).Publish(Arg.Any<OnMilitaryTokenReachedThreshold>());
            m_eventManager.Received(1).Publish(Arg.Any<MilitaryVictory>());
        }

        [Test]
        public void When_Initalize_Called_OnMilitaryAdvanced_Event_Happens_Player2_Advances()
        {
            OnMilitaryAdvanced onMilitaryAdvanced = new OnMilitaryAdvanced(m_player2, 10);
            m_eventManager.When(evt => evt.Subscribe(Arg.Any<Action<OnMilitaryAdvanced>>())).Do(callinfo => ((Action<OnMilitaryAdvanced>)callinfo[0])(onMilitaryAdvanced));

            m_militaryBoard.Initialize([m_player1, m_player2], [], m_gameContext);

            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnMilitaryAdvanced>>());
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnScientificProgress>>());
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnMilitaryTokenReachedThreshold>>());
            Assert.That(m_militaryBoard.Fields[5] == MilitaryField.None, Is.True);
            Assert.That(m_militaryBoard.Fields[0] == MilitaryField.Shield, Is.True);
            m_eventManager.Received(1).Publish(Arg.Any<OnMilitaryTokenReachedThreshold>());
            m_eventManager.Received(1).Publish(Arg.Any<MilitaryVictory>());
        }

        [Test]
        public void When_Initalize_Called_OnScientificProgress_Event_Happens()
        {
            m_playerAction.CanPerform(m_gameContext).Returns(true);
            m_player1.Cards.AddRange(
            [
                new GreenCard() { Discipline = new Building() },
                new GreenCard() { Discipline = new Geography() },
                new GreenCard() { Discipline = new Healing() },
                new GreenCard() { Discipline = new Mechanics() },
                new GreenCard() { Discipline = new Physics() },
                new GreenCard() { Discipline = new Trading() },
                new GreenCard() { Discipline = new Building() },
            ]);

            OnScientificProgress onScientificProgress = new OnScientificProgress(m_player1, new Building(), m_playerActionReceiver);
            m_eventManager.When(evt => evt.Subscribe(Arg.Any<Action<OnScientificProgress>>())).Do(callinfo => ((Action<OnScientificProgress>)callinfo[0])(onScientificProgress));

            m_militaryBoard.Initialize([m_player1, m_player2], [], m_gameContext);

            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnMilitaryAdvanced>>());
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnScientificProgress>>());
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnMilitaryTokenReachedThreshold>>());
            m_playerActionReceiver.Received(1).ReceivePlayerAction(m_player1, Arg.Any<ICollection<IPlayerAction>>());
            m_playerAction.Received(1).CanPerform(m_gameContext);
            m_playerAction.Received(1).DoPlayerAction(m_gameContext);
            m_eventManager.Received(1).Publish(Arg.Any<ScientificVictory>());

        }

        [Test]
        public void When_Initalize_Called_OnMilitaryTokenReachedThreshold_Event_Happens()
        {
            List<MilitaryCard> militaryCards = [.. m_militaryBoard.MilitaryCards];
            OnMilitaryTokenReachedThreshold onMilitaryTokenReachedThreshold = new OnMilitaryTokenReachedThreshold(militaryCards);
            m_eventManager.When(evt => evt.Subscribe(Arg.Any<Action<OnMilitaryTokenReachedThreshold>>())).Do(callinfo => ((Action<OnMilitaryTokenReachedThreshold>)callinfo[0])(onMilitaryTokenReachedThreshold));

            m_militaryBoard.Initialize([m_player1, m_player2], [], m_gameContext);

            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnMilitaryAdvanced>>());
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnScientificProgress>>());
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnMilitaryTokenReachedThreshold>>());
            Assert.That(militaryCards.All(card => !m_militaryBoard.MilitaryCards.Contains(card)), Is.True);
            Assert.That(m_militaryBoard.MilitaryCards.Count, Is.EqualTo(0));
        }

        private Player m_player1;
        private Player m_player2;
        private IGameContext m_gameContext;
        private IEventManager m_eventManager;
        private IPlayerActionReceiver m_playerActionReceiver;
        private IPlayerAction m_playerAction;
        private MilitaryBoard m_militaryBoard;
    }
}
