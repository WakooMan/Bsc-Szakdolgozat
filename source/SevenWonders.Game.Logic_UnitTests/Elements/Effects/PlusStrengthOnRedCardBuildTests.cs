using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Effects;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.Handlers;
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

            m_plusStrengthOnRedCardBuild.Apply(m_gameContext, m_player, new Player());

            m_player.OnBuildCard(onCardBuilt);

            Assert.That(redCard.Strength.Points, Is.EqualTo(point + m_plusStrengthOnRedCardBuild.AdditionalStrength.Points));
        }

        private IGameContext m_gameContext;
        private ITurnHandler m_turnHandler;
        private IEventManager m_eventManager;
        private Player m_player;
        private PlusStrengthOnRedCardBuild m_plusStrengthOnRedCardBuild;
    }
}
