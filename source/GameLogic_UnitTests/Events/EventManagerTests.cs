using GameLogic.Elements;
using GameLogic.Events;
using GameLogic.Events.GameEvents;

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
            m_eventManager.Subscribe((OnBuildingCostCalculated calculated) => called++);
            m_eventManager.Publish(new OnBuildingCostCalculated(new Player()));

            Assert.That(called, Is.EqualTo(1));
        }

        [Test]
        public void When_Subscribe_Called_And_Publish_Called_After_ClearSubscriptions()
        {
            int called = 0;
            m_eventManager.Subscribe((OnBuildingCostCalculated calculated) => called++);
            m_eventManager.ClearSubscriptions();
            m_eventManager.Publish(new OnBuildingCostCalculated(new Player()));

            Assert.That(called, Is.EqualTo(0));
        }

        [Test]
        public void When_Subscribe_Called_And_Publish_Called_After_Unsubscribe()
        {
            int called = 0;
            Action<OnBuildingCostCalculated> action =(calculated) => called++;
            m_eventManager.Subscribe(action);
            m_eventManager.Unsubscribe(action);
            m_eventManager.Publish(new OnBuildingCostCalculated(new Player()));

            Assert.That(called, Is.EqualTo(0));
        }

        private EventManager m_eventManager;
    }
}
