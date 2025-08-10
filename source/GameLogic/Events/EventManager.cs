using GameLogic.Events.GameEvents;
using System.ComponentModel.Composition;

namespace GameLogic.Events
{
    [Export(typeof(IEventManager))]
    public class EventManager : IEventManager
    {
        private readonly Dictionary<Type, List<Delegate>> _listeners = new();

        [ImportingConstructor]
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
                foreach (var del in list)
                {
                    if (del is Action<TGameEvent> action)
                    {
                        action(eventArgs);
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
