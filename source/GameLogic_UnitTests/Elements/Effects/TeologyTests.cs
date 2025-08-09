using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.Handlers;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class TeologyTests
    {
        [SetUp]
        public void Setup()
        {
            m_gameContext = Substitute.For<IGameContext>();
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_eventManager = Substitute.For<IEventManager>();
            m_player = new Player();
            m_gameContext.EventManager.Returns(m_eventManager);
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_turnHandler.CurrentPlayer.Returns(m_player);
            m_teology = new Teology();
        }

        [Test]
        public void When_Clone_Called()
        {
            Teology teology = m_teology.Clone();

            Assert.That(teology, Is.Not.Null);
            Assert.That(teology, Is.Not.EqualTo(m_teology));
        }

        [Test]
        public void When_Apply_Called_And_Wonder_Does_Not_Have_NewTurn_Effect()
        {
            Wonder wonder = new Wonder();
            OnWonderBuilt onWonderBuilt = new OnWonderBuilt(m_player, Substitute.For<Card>(), wonder);
            m_eventManager.When((evt) => evt.Subscribe(Arg.Any<Action<OnWonderBuilt>>())).Do((callinfo) => ((Action<OnWonderBuilt>)callinfo[0])(onWonderBuilt));

            m_teology.Apply(m_gameContext);

            _ = m_turnHandler.Received(1).CurrentPlayer;
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnWonderBuilt>>());
            Assert.That(wonder.Effects.Any(eff => eff is NewTurn), Is.True);
        }

        [Test]
        public void When_Apply_Called_And_Wonder_Does_Not_Have_NewTurn_Effect_And_Builder_Is_Different()
        {
            Wonder wonder = new Wonder();
            OnWonderBuilt onWonderBuilt = new OnWonderBuilt(new Player(), Substitute.For<Card>(), wonder);
            m_eventManager.When((evt) => evt.Subscribe(Arg.Any<Action<OnWonderBuilt>>())).Do((callinfo) => ((Action<OnWonderBuilt>)callinfo[0])(onWonderBuilt));

            m_teology.Apply(m_gameContext);

            _ = m_turnHandler.Received(1).CurrentPlayer;
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnWonderBuilt>>());
            Assert.That(wonder.Effects.Any(eff => eff is NewTurn), Is.False);
        }

        [Test]
        public void When_Apply_Called_And_Wonder_Has_NewTurn_Effect()
        {
            Wonder wonder = new Wonder();
            NewTurn newTurn = new NewTurn();
            wonder.Effects.Add(newTurn);
            OnWonderBuilt onWonderBuilt = new OnWonderBuilt(m_player, Substitute.For<Card>(), wonder);
            m_eventManager.When((evt) => evt.Subscribe(Arg.Any<Action<OnWonderBuilt>>())).Do((callinfo) => ((Action<OnWonderBuilt>)callinfo[0])(onWonderBuilt));

            m_teology.Apply(m_gameContext);

            _ = m_turnHandler.Received(1).CurrentPlayer;
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnWonderBuilt>>());
            Assert.That(wonder.Effects.Count(eff => eff is NewTurn), Is.EqualTo(1));
            Assert.That(wonder.Effects.Contains(newTurn), Is.True);
        }

        private IGameContext m_gameContext;
        private IEventManager m_eventManager;
        private ITurnHandler m_turnHandler;
        private Player m_player;
        private Teology m_teology;
    }
}
