using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Developments;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Modifiers;
using GameLogic.Handlers;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
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
            m_playerAction = Substitute.For<IPlayerAction>();
            m_playerActionReceiver = Substitute.For<IPlayerActionReceiver>();
            m_randomGenerator = Substitute.For<IRandomGenerator>();
            m_player = new Player();
            m_turnHandler.CurrentPlayer.Returns(m_player);
            m_playerAction.CanPerform(m_gameContext).Returns(true);
            m_playerActionReceiver.ReceivePlayerAction(m_player, Arg.Any<ICollection<IPlayerAction>>()).Returns(m_playerAction);
            m_gameContext.PlayerActionReceiver.Returns(m_playerActionReceiver);
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_gameContext.DevelopmentList.Returns(m_developmentList);
            m_gameContext.RandomGenerator.Returns(m_randomGenerator);
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
            m_randomGenerator.Next().Returns(1);

            Assert.Throws<InvalidOperationException>(()=>m_chooseDevelopment.Apply(m_gameContext));

            m_randomGenerator.DidNotReceive().Next();
            m_playerActionReceiver.DidNotReceive().ReceivePlayerAction(m_player, Arg.Is<ICollection<IPlayerAction>>(x => x.Count == 3));
            m_playerAction.DidNotReceive().CanPerform(m_gameContext);
            m_playerAction.DidNotReceive().DoPlayerAction(m_gameContext);
        }

        [Test]
        public void When_Apply_Called_Three_Developments()
        {
            List<Development> developments = [new Development() { Name = "development1" }, new Development() { Name = "development2" }, new Development() { Name = "development3" }];
            m_developmentList.Developments.Returns(developments);
            m_randomGenerator.Next().Returns(1);

            m_chooseDevelopment.Apply(m_gameContext);

            m_randomGenerator.Received(developments.Count).Next();
            m_playerActionReceiver.Received(1).ReceivePlayerAction(m_player, Arg.Is<ICollection<IPlayerAction>>(x => x.Count == 3));
            m_playerAction.Received(1).CanPerform(m_gameContext);
            m_playerAction.Received(1).DoPlayerAction(m_gameContext);
        }

        [Test]
        public void When_Apply_Called_Two_Developments()
        {
            List<Development> developments = [new Development() { Name = "development2" }, new Development() { Name = "development3" }];
            m_developmentList.Developments.Returns(developments);
            m_randomGenerator.Next().Returns(1);

            m_chooseDevelopment.Apply(m_gameContext);

            m_randomGenerator.Received(developments.Count).Next();
            m_playerActionReceiver.Received(1).ReceivePlayerAction(m_player, Arg.Is<ICollection<IPlayerAction>>(x => x.Count == 2));
            m_playerAction.Received(1).CanPerform(m_gameContext);
            m_playerAction.Received(1).DoPlayerAction(m_gameContext);
        }

        [Test]
        public void When_Apply_Called_Four_Developments()
        {
            List<Development> developments = [new Development() { Name = "development1" }, new Development() { Name = "development2" }, new Development() { Name = "development3" }, new Development() { Name = "development4" },];
            m_developmentList.Developments.Returns(developments);
            m_randomGenerator.Next().Returns(1);

            m_chooseDevelopment.Apply(m_gameContext);

            m_randomGenerator.Received(developments.Count).Next();
            m_playerActionReceiver.Received(1).ReceivePlayerAction(m_player, Arg.Is<ICollection<IPlayerAction>>(x => x.Count == 3));
            m_playerAction.Received(1).CanPerform(m_gameContext);
            m_playerAction.Received(1).DoPlayerAction(m_gameContext);
        }

        private IRandomGenerator m_randomGenerator;
        private IDevelopmentList m_developmentList;
        private IGameContext m_gameContext;
        private ITurnHandler m_turnHandler;
        private IPlayerAction m_playerAction;
        private IPlayerActionReceiver m_playerActionReceiver;
        private Player m_player;
        private ChooseDevelopment m_chooseDevelopment;
    }
}
