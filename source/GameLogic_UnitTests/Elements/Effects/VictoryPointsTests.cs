using GameLogic;
using GameLogic.Elements.Effects;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class VictoryPointsTests
    {
        [SetUp]
        public void Setup()
        {
            m_gameContext = Substitute.For<IGameContext>();
            m_eventManager = Substitute.For<IEventManager>();
            m_gameContext.EventManager.Returns(m_eventManager);
            m_victoryPoints = new VictoryPoints();
            m_victoryPoints.Points = 5;
        }

        [Test]
        public void When_Clone_Called()
        {
            VictoryPoints victoryPoints = m_victoryPoints.Clone();

            Assert.That(victoryPoints, Is.Not.Null);
            Assert.That(victoryPoints, Is.Not.EqualTo(m_victoryPoints));
            Assert.That(victoryPoints.Points, Is.EqualTo(m_victoryPoints.Points));
        }

        [Test]
        public void When_Apply_Called()
        {
            m_victoryPoints.Apply(m_gameContext);
        }

        private IGameContext m_gameContext;
        private IEventManager m_eventManager;
        private VictoryPoints m_victoryPoints;
    }
}
