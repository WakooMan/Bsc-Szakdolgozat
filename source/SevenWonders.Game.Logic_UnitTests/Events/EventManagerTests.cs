using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Events.GameEvents;

namespace GameLogic_UnitTests.Events
{
    public class EventManagerTests
    {
        [SetUp]
        public void Setup()
        {
            m_eventManager = new EventManager();
        }

        [Test]
        public void When_Subscribe_And_Publish_Called()
        {
            int called = 0;
            m_eventManager.Subscribe((TurnStarted started) => called++);
            m_eventManager.Publish(new TurnStarted(new Player()));

            Assert.That(called, Is.EqualTo(1));
        }

        [Test]
        public void When_Subscribe_Called_And_Publish_Called_After_ClearSubscriptions()
        {
            int called = 0;
            m_eventManager.Subscribe((TurnStarted started) => called++);
            m_eventManager.ClearSubscriptions();
            m_eventManager.Publish(new TurnStarted(new Player()));

            Assert.That(called, Is.EqualTo(0));
        }

        [Test]
        public void When_Subscribe_Called_And_Publish_Called_After_Unsubscribe()
        {
            int called = 0;
            Action<TurnStarted> action = (started) => called++;
            m_eventManager.Subscribe(action);
            m_eventManager.Unsubscribe(action);
            m_eventManager.Publish(new TurnStarted(new Player()));

            Assert.That(called, Is.EqualTo(0));
        }

        private EventManager m_eventManager;
    }
}
