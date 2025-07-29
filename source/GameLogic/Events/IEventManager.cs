namespace GameLogic.Events
{
    public interface IEventManager
    {
        void Subscribe(GameEventType eventType, Action<EventArgs> listener);
        void Publish(GameEventType eventType, EventArgs game);
        bool Unsubscribe(GameEventType eventType, Action<EventArgs> listener);
    }
}
