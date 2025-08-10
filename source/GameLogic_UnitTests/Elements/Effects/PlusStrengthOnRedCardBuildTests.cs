using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.Handlers;
using NSubstitute;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class PlusStrengthOnRedCardBuildTests
    {
        [SetUp]
        public void Setup()
        {
            m_gameContext = Substitute.For<IGameContext>();
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_eventManager = Substitute.For<IEventManager>();
            m_player = new Player();
            m_turnHandler.CurrentPlayer.Returns(m_player);
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_gameContext.EventManager.Returns(m_eventManager);
            m_plusStrengthOnRedCardBuild = new PlusStrengthOnRedCardBuild();
            m_plusStrengthOnRedCardBuild.AdditionalStrength.Points = 2;
        }

        [Test]
        public void When_Clone_Called()
        {
            PlusStrengthOnRedCardBuild plusStrengthOnRedCardBuild = m_plusStrengthOnRedCardBuild.Clone();

            Assert.That(plusStrengthOnRedCardBuild, Is.Not.Null);
            Assert.That(m_plusStrengthOnRedCardBuild, Is.Not.EqualTo(plusStrengthOnRedCardBuild));
            Assert.That(plusStrengthOnRedCardBuild.AdditionalStrength.Points, Is.EqualTo(m_plusStrengthOnRedCardBuild.AdditionalStrength.Points));
        }

        [Test]
        public void When_Apply_Called()
        {
            int point = 3;
            RedCard redCard = new RedCard();
            redCard.Strength.Points = point;
            OnCardBuilt onCardBuilt = new OnCardBuilt(redCard, m_player, 2, false);
            m_eventManager.When((evt) => evt.Subscribe(Arg.Any<Action<OnCardBuilt>>())).Do((callinfo) => ((Action<OnCardBuilt>)callinfo[0])(onCardBuilt));

            m_plusStrengthOnRedCardBuild.Apply(m_gameContext);

            _ = m_turnHandler.Received(1).CurrentPlayer;
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnCardBuilt>>());
            Assert.That(redCard.Strength.Points, Is.EqualTo(point + m_plusStrengthOnRedCardBuild.AdditionalStrength.Points));
        }

        private IGameContext m_gameContext;
        private ITurnHandler m_turnHandler;
        private IEventManager m_eventManager;
        private Player m_player;
        private PlusStrengthOnRedCardBuild m_plusStrengthOnRedCardBuild;
    }
}
