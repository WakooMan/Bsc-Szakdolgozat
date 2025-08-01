using GameLogic.Events.GameEvents;

namespace GameLogic.Events
{
    public interface IEventManager
    {
        void Subscribe<TGameEvent>(Action<TGameEvent> listener) where TGameEvent : GameEvent;
        void Publish<TGameEvent>(TGameEvent eventArgs) where TGameEvent : GameEvent;
        bool Unsubscribe<TGameEvent>(Action<TGameEvent> listener) where TGameEvent : GameEvent;
        void ClearSubscriptions();
    }
}
