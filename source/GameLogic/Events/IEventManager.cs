namespace GameLogic.Events
{
    public interface IEventManager
    {
        void Subscribe<TEventArgs>(GameEventType eventType, Action<TEventArgs> listener) where TEventArgs : EventArgs;
        void Publish<TEventArgs>(GameEventType eventType, TEventArgs eventArgs) where TEventArgs : EventArgs;
        bool Unsubscribe<TEventArgs>(GameEventType eventType, Action<TEventArgs> listener) where TEventArgs : EventArgs;
        void ClearSubscriptions();
    }
}
