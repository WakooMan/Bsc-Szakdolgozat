namespace GameLogic.Events
{
    public class EventManager : IEventManager
    {
        private readonly Dictionary<GameEventType, List<Action<EventArgs>>> _listeners = new();

        public EventManager() { }

        public void Subscribe(GameEventType eventType, Action<EventArgs> listener)
        {
            if (!_listeners.ContainsKey(eventType))
                _listeners[eventType] = new List<Action<EventArgs>>();

            _listeners[eventType].Add(listener);
        }

        public void Publish(GameEventType eventType, EventArgs game)
        {
            if (_listeners.TryGetValue(eventType, out var list))
            {
                foreach (var listener in list)
                    listener(game);
            }
        }

        public bool Unsubscribe(GameEventType eventType, Action<EventArgs> listener)
        {
            if (!_listeners.ContainsKey(eventType))
            {
                return false;
            }

            return _listeners[eventType].Remove(listener);
        }
    }
}
