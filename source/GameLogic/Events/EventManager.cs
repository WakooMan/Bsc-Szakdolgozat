using GameLogic.Events.GameEvents;

namespace GameLogic.Events
{
    public class EventManager : IEventManager
    {
        private readonly Dictionary<Type, List<Delegate>> _listeners = new();

        public EventManager() { }

        public void Subscribe<TGameEvent>(Action<TGameEvent> listener) where TGameEvent : GameEvent
        {
            if (!_listeners.TryGetValue(typeof(TGameEvent), out var list))
            {
                list = new List<Delegate>();
                _listeners[typeof(TGameEvent)] = list;
            }

            list.Add(listener);
        }

        public Task PublishAsync<TGameEvent>(TGameEvent eventArgs) where TGameEvent : GameEvent
        {
            if (_listeners.TryGetValue(typeof(TGameEvent), out var list))
            {
                var tasks = list
                    .OfType<Action<TGameEvent>>()
                    .Select(action => Task.Run(() => action(eventArgs)))
                    .ToArray();

                return Task.WhenAll(tasks);
            }

            return Task.CompletedTask;
        }

        public bool Unsubscribe<TGameEvent>(Action<TGameEvent> listener) where TGameEvent : GameEvent
        {
            if (!_listeners.TryGetValue(typeof(TGameEvent), out var list))
                return false;

            return list.Remove(listener);
        }

        public void ClearSubscriptions()
        {
            _listeners.Clear();
        }
    }
}
