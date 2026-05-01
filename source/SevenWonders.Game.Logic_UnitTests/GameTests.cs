using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Ages;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Elements.Modifiers;
using SevenWonders.Game.Logic.Elements.Wonders;
using SevenWonders.Game.Logic.GameStates;
using SevenWonders.Game.Logic.GameStructures;
using SevenWonders.Game.Logic.GameStructures.Factories;
using SevenWonders.Game.Logic.Handlers;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.Game.Logic.Events;
using NSubstitute;
using SevenWonders.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SevenWonders.Game.Logic.Elements.Developments;

namespace GameLogic_UnitTests
{
    public class GameTests
    {
        [SetUp]
        public void Setup()
        {
            m_eventManager = Substitute.For<IEventManager>();
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_ageHandler = Substitute.For<IAgeHandler>();
            m_chooseWonderHandler = Substitute.For<IChooseWonderHandler>();
            m_gameContext = Substitute.For<IGameContext>();
            m_developmentList = Substitute.For<IDevelopmentList>();
            m_developmentList.Developments.Returns([new Development(),
                                                    new Development(),
                                                    new Development(),
                                                    new Development(),
                                                    new Development(),
                                                    new Development(),
                                                    new Development(),
                                                    new Development()]);
            m_wonderList = Substitute.For<IWonderList>();
            m_wonderList.Wonders.Returns([new Wonder(),
                                          new Wonder(),
                                          new Wonder(),
                                          new Wonder(),
                                          new Wonder(),
                                          new Wonder(),
                                          new Wonder(),
                                          new Wonder()]);
            m_gameContext.DevelopmentList.Returns(m_developmentList);
            m_gameContext.WonderList.Returns(m_wonderList);
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
            IRandomGenerator randomGenerator = Substitute.For<IRandomGenerator>();
            randomGenerator.ReceiveRandomElements(Arg.Any<ICollection<Card>>(), 20).Returns(callInfo => callInfo.ArgAt<ICollection<Card>>(0));
            randomGenerator.ReceiveRandomElements(Arg.Any<ICollection<Wonder>>(), 8).Returns(callInfo => callInfo.ArgAt<ICollection<Wonder>>(0));
            AgeBase currentAge = new FirstAge(m_eventManager, cardCompositionFactory, cardList, randomGenerator);
            m_ageHandler.CurrentAge.Returns(currentAge);
            m_chooseWonderHandler.WondersChosen.Returns(true);
            m_ageHandler.NextAge().Returns(false);
            IPlayerActionReceiver actionReceiver1 = Substitute.For<IPlayerActionReceiver>();
            IPlayerActionReceiver actionReceiver2 = Substitute.For<IPlayerActionReceiver>();
            m_game.Initialize(randomGenerator, ("player1", actionReceiver1), ("player2", actionReceiver2));
            m_turnHandler.GetPlayer(1).Returns(m_game.Players[0]);
            m_turnHandler.GetPlayer(2).Returns(m_game.Players[1]);
            m_gameContext.Received(1).Initialize(Arg.Any<ICollection<Player>>(), Arg.Any<IRandomGenerator>());
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

        private IEventManager m_eventManager;
        private ITurnHandler m_turnHandler;
        private IAgeHandler m_ageHandler;
        private IChooseWonderHandler m_chooseWonderHandler;
        private IGameContext m_gameContext;
        private IDevelopmentList m_developmentList;
        private IWonderList m_wonderList;
        private Game m_game;
    }
}
