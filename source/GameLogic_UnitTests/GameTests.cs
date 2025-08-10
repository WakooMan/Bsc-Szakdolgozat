using GameLogic;
using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Modifiers;
using GameLogic.Elements.Wonders;
using GameLogic.GameStates;
using GameLogic.GameStructures;
using GameLogic.GameStructures.Factories;
using GameLogic.Handlers;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests
{
    public class GameTests
    {
        [SetUp]
        public void Setup()
        {
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_ageHandler = Substitute.For<IAgeHandler>();
            m_chooseWonderHandler = Substitute.For<IChooseWonderHandler>();
            m_gameContext = Substitute.For<IGameContext>();
            m_gameContext.ChooseWonderHandler.Returns(m_chooseWonderHandler);
            m_gameContext.AgeHandler.Returns(m_ageHandler);
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_game = new Game(m_gameContext);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new Game(null));
        }

        [Test]
        public void When_Constructor_Called()
        {
            Assert.That(m_game.CurrentState is EndGameState, Is.True);
            Assert.That(m_game.Players.Count, Is.EqualTo(0));
            Assert.That(m_game.IsInitialized, Is.False);
        }

        [Test]
        public void When_GameLoop_Called_Without_Initialize()
        {
            Assert.Throws<InvalidOperationException>(m_game.GameLoop);
        }

        [Test]
        public void When_GameLoop_Called_With_Initialize()
        {
            ICardComposition cardComposition = Substitute.For<ICardComposition>();
            cardComposition.AvailableCards.Returns([]);
            ICardCompositionFactory cardCompositionFactory = Substitute.For<ICardCompositionFactory>();
            cardCompositionFactory.Create(Arg.Any<string>(), Arg.Any<ICollection<Card>>()).Returns(cardComposition);
            ICardList cardList = Substitute.For<ICardList>();
            cardList.Cards.Returns([]);
            AgeBase currentAge = new FirstAge(cardCompositionFactory, cardList);
            m_ageHandler.CurrentAge.Returns(currentAge);
            m_chooseWonderHandler.WondersChosen.Returns(true);
            m_ageHandler.NextAge().Returns(false);
            m_game.Initialize("player1", "player2", [], []);
            m_gameContext.Received(1).Initialize(Arg.Any<ICollection<Player>>(), Arg.Any<ICollection<Wonder>>(), Arg.Any<ICollection<Development>>());
            Assert.That(m_game.CurrentState is ChooseWonderState, Is.True);
            Assert.That(m_game.IsInitialized, Is.True);
            Assert.That(m_game.Players.Count, Is.EqualTo(2));
            m_turnHandler.CurrentPlayer.Returns(m_game.Players[0]);
            m_turnHandler.OpponentPlayer.Returns(m_game.Players[1]);

            m_game.GameLoop();

            Assert.That(m_game.CurrentState is EndGameState, Is.True);
            Assert.That(m_game.IsInitialized, Is.False);
            Assert.Throws<InvalidOperationException>(m_game.GameLoop);
        }

        private ITurnHandler m_turnHandler;
        private IAgeHandler m_ageHandler;
        private IChooseWonderHandler m_chooseWonderHandler;
        private IGameContext m_gameContext;
        private Game m_game;
    }
}
