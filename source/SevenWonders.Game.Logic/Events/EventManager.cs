using SevenWonders.Game.Logic.Events.GameEvents;

namespace SevenWonders.Game.Logic.Events
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

        public void Publish<TGameEvent>(TGameEvent eventArgs) where TGameEvent : GameEvent
        {
            if (_listeners.TryGetValue(typeof(TGameEvent), out var list))
            {
                foreach (var listener in list)
                {
                    try
                    {
                        listener?.DynamicInvoke(eventArgs);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error invoking listener for {typeof(TGameEvent).Name}: {ex}");
                    }
                }
            }
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
