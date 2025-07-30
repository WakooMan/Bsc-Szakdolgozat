using System.ComponentModel.Composition;

namespace GameLogic.Events
{
    [Export(typeof(IEventManager))]
    public class EventManager : IEventManager
    {
        private readonly Dictionary<GameEventType, List<Delegate>> _listeners = new();

        [ImportingConstructor]
        public EventManager() { }

        public void Subscribe<TEventArgs>(GameEventType eventType, Action<TEventArgs> listener) where TEventArgs : EventArgs
        {
            if (!_listeners.TryGetValue(eventType, out var list))
            {
                list = new List<Delegate>();
                _listeners[eventType] = list;
            }

            list.Add(listener);
        }

        public void Publish<TEventArgs>(GameEventType eventType, TEventArgs eventArgs) where TEventArgs : EventArgs
        {
            if (_listeners.TryGetValue(eventType, out var list))
            {
                foreach (var del in list)
                {
                    if (del is Action<TEventArgs> action)
                    {
                        action(eventArgs);
                    }
                }
            }
        }

        public bool Unsubscribe<TEventArgs>(GameEventType eventType, Action<TEventArgs> listener) where TEventArgs : EventArgs
        {
            if (!_listeners.TryGetValue(eventType, out var list))
                return false;

            return list.Remove(listener);
        }

        public void ClearSubscriptions()
        {
            _listeners.Clear();
        }
    }
}
