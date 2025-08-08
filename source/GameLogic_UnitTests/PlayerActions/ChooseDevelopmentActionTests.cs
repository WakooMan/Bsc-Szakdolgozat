using GameLogic;
using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Modifiers;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.GameStructures;
using GameLogic.Handlers;
using GameLogic.PlayerActions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.PlayerActions
{
    public class ChooseDevelopmentActionTests
    {
        [SetUp]
        public void Setup()
        {
            m_development = Substitute.For<Development>();
            m_costCalculator = Substitute.For<ICostCalculator>();
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_gameContext = Substitute.For<IGameContext>();
            m_cardComposition = Substitute.For<ICardComposition>();
            m_eventManager = Substitute.For<IEventManager>();
            m_age = Substitute.For<IAgeBase>();
            m_ageHandler = Substitute.For<IAgeHandler>();
            m_current = new Player() { Name = "Current" };
            m_opponent = new Player() { Name = "Opponent" };
            m_turnHandler.CurrentPlayer.Returns(m_current);
            m_turnHandler.OpponentPlayer.Returns(m_opponent);
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_gameContext.CostCalculator.Returns(m_costCalculator);
            m_gameContext.AgeHandler.Returns(m_ageHandler);
            m_gameContext.EventManager.Returns(m_eventManager);
            m_ageHandler.CurrentAge.Returns(m_age);
            m_age.Composition.Returns(m_cardComposition);
            m_chooseDevelopmentAction = new ChooseDevelopmentAction(m_current, m_development);
        }

        [Test]
        public void When_Constructor_Called_With_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new ChooseDevelopmentAction(null, m_development));
            Assert.Throws<ArgumentNullException>(() => new ChooseDevelopmentAction(m_current, null));
        }

        [Test]
        public void When_CanPerform_Called_Player_Has_Development()
        {
            m_current.Developments.Add(m_development);

            bool result = m_chooseDevelopmentAction.CanPerform(m_gameContext);

            Assert.That(result, Is.False);
        }

        [Test]
        public void When_CanPerform_Called_Returns_True()
        {

            bool result = m_chooseDevelopmentAction.CanPerform(m_gameContext);

            Assert.That(result, Is.True);
        }

        [Test]
        public void When_DoPlayerAction_Called_And_Player_Has_Development()
        {
            m_current.Developments.Add(m_development);

            Assert.Throws<InvalidOperationException>(() => m_chooseDevelopmentAction.DoPlayerAction(m_gameContext));
        }

        [Test]
        public void When_DoPlayerAction_Called_Successful()
        {
            m_chooseDevelopmentAction.DoPlayerAction(m_gameContext);

            Assert.That(m_current.Developments.Contains(m_development), Is.True);
            m_development.Received(1).OnDevelopmentEstablished(m_gameContext);
        }

        private ChooseDevelopmentAction m_chooseDevelopmentAction;
        private Development m_development;
        private Player m_current;
        private Player m_opponent;
        private ICostCalculator m_costCalculator;
        private ITurnHandler m_turnHandler;
        private IGameContext m_gameContext;
        private IAgeHandler m_ageHandler;
        private IAgeBase m_age;
        private ICardComposition m_cardComposition;
        private IEventManager m_eventManager;
    }
}
