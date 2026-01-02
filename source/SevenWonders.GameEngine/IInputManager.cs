using SkiaSharp.Views.Maui;

namespace SevenWonders.GameEngine
{
    public interface IInputManager
    {
        void OnTouchEvent(SKTouchEventArgs touchEventArgs);
        void SubscribeTouchEvent(TouchEvent touchEvent, SKMouseButton mouseButton, Action<SKTouchEventArgs> action);
        void UnsubscribeTouchEvent(TouchEvent mouseEvent, SKMouseButton mouseButton, Action<SKTouchEventArgs> action);
    }
}
