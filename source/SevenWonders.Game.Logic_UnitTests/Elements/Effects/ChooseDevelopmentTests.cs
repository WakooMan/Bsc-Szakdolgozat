using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Developments;
using SevenWonders.Game.Logic.Elements.Effects;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Elements.Modifiers;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.Handlers;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.Game.Logic.PlayerActions;
using NSubstitute;
using SevenWonders.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class ChooseDevelopmentTests
    {
        [SetUp]
        public void Setup()
        {
            m_developmentList = Substitute.For<IDevelopmentList>();
            m_gameContext = Substitute.For<IGameContext>();
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_playerActionHandler = Substitute.For<IPlayerActionHandler>();
            m_randomGenerator = Substitute.For<IRandomGenerator>();
            m_eventManager = Substitute.For<IEventManager>();
            m_player = new Player();
            m_opponent = new Player();
            m_turnHandler.CurrentPlayer.Returns(m_player);
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_gameContext.DevelopmentList.Returns(m_developmentList);
            m_gameContext.RandomGenerator.Returns(m_randomGenerator);
            m_gameContext.PlayerActionHandler.Returns(m_playerActionHandler);
            m_gameContext.EventManager.Returns(m_eventManager);
            m_chooseDevelopment = new ChooseDevelopment();
        }

        [Test]
        public void When_Clone_Called()
        {
            ChooseDevelopment chooseDevelopment = m_chooseDevelopment.Clone();

            Assert.That(chooseDevelopment, Is.Not.Null);
            Assert.That(m_chooseDevelopment, Is.Not.EqualTo(chooseDevelopment));
        }

        [Test]
        public void When_Apply_Called_And_DevelopmentList_Is_Null()
        {
            m_gameContext.DevelopmentList.Returns(null as IDevelopmentList);

            Assert.Throws<InvalidOperationException>(()=>m_chooseDevelopment.Apply(m_gameContext, m_player, m_opponent));

            m_randomGenerator.DidNotReceive().TryReceiveRandomElements(Arg.Any<ICollection<Development>>(), Arg.Any<int>());
            m_playerActionHandler.DidNotReceive().HandlePlayerActions(m_gameContext, m_player, Arg.Any<ICollection<IPlayerAction>>());
        }

        [Test]
        public void When_Apply_Called_Three_Developments()
        {
            List<Development> developments = [new Development() { Name = "development1" }, new Development() { Name = "development2" }, new Development() { Name = "development3" }];
            m_developmentList.Developments.Returns(developments);
            m_randomGenerator.TryReceiveRandomElements(Arg.Any<ICollection<Development>>(), 3).Returns(developments);

            m_chooseDevelopment.Apply(m_gameContext, m_player, m_opponent);

            m_randomGenerator.Received(1).TryReceiveRandomElements(Arg.Any<ICollection<Development>>(), 3);
            m_eventManager.Received(1).Publish(Arg.Any<OnChooseObjects>());
            m_playerActionHandler.Received(1).HandlePlayerActions(m_gameContext, m_player, Arg.Is<ICollection<IPlayerAction>>(x => x.Count == 3));
        }

        [Test]
        public void When_Apply_Called_Two_Developments()
        {
            List<Development> developments = [new Development() { Name = "development2" }, new Development() { Name = "development3" }];
            m_developmentList.Developments.Returns(developments);
            m_randomGenerator.TryReceiveRandomElements(Arg.Any<ICollection<Development>>(), 3).Returns(developments);

            m_chooseDevelopment.Apply(m_gameContext, m_player, m_opponent);

            m_randomGenerator.Received(1).TryReceiveRandomElements(Arg.Any<ICollection<Development>>(), 3);
            m_eventManager.Received(1).Publish(Arg.Any<OnChooseObjects>());
            m_playerActionHandler.Received(1).HandlePlayerActions(m_gameContext, m_player, Arg.Is<ICollection<IPlayerAction>>(x => x.Count == 2));
        }

        [Test]
        public void When_Apply_Called_Four_Developments()
        {
            List<Development> developments = [new Development() { Name = "development1" }, new Development() { Name = "development2" }, new Development() { Name = "development3" }, new Development() { Name = "development4" },];
            m_developmentList.Developments.Returns(developments);
            m_randomGenerator.TryReceiveRandomElements(Arg.Any<ICollection<Development>>(), 3).Returns(developments.Take(3).ToList());

            m_chooseDevelopment.Apply(m_gameContext, m_player, m_opponent);

            m_randomGenerator.Received(1).TryReceiveRandomElements(Arg.Any<ICollection<Development>>(), 3);
            m_eventManager.Received(1).Publish(Arg.Any<OnChooseObjects>());
            m_playerActionHandler.Received(1).HandlePlayerActions(m_gameContext, m_player, Arg.Is<ICollection<IPlayerAction>>(x => x.Count == 3));
        }

        private IRandomGenerator m_randomGenerator;
        private IDevelopmentList m_developmentList;
        private IGameContext m_gameContext;
        private ITurnHandler m_turnHandler;
        private IPlayerActionHandler m_playerActionHandler;
        private IEventManager m_eventManager;
        private Player m_player;
        private Player m_opponent;
        private ChooseDevelopment m_chooseDevelopment;
    }
}
