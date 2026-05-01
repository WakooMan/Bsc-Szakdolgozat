using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Effects;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Events.GameEvents;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class StrengthTests
    {
        [SetUp]
        public void Setup()
        {
            m_gameContext = Substitute.For<IGameContext>();
            m_eventManager = Substitute.For<IEventManager>();
            m_gameContext.EventManager.Returns(m_eventManager);
            m_strength = new Strength();
            m_strength.Points = 5;
        }

        [Test]
        public void When_Clone_Called()
        {
            Strength strength = m_strength.Clone();

            Assert.That(strength, Is.Not.Null);
            Assert.That(strength, Is.Not.EqualTo(m_strength));
            Assert.That(strength.Points, Is.EqualTo(m_strength.Points));
        }

        [Test]
        public void When_Apply_Called()
        {
            m_strength.Apply(m_gameContext, new Player(), new Player());
        }

        private IGameContext m_gameContext;
        private IEventManager m_eventManager;
        private Strength m_strength;
    }
}
