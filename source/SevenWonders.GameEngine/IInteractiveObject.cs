using SkiaSharp.Views.Maui;

namespace SevenWonders.GameEngine
{
    public interface IInteractiveObject
    {
        delegate void TouchEvent(IInteractiveObject interactiveObject, SKTouchEventArgs eventArgs);

        event TouchEvent ReleasedEvent;
        event TouchEvent PressedEvent;
        event TouchEvent MoveEvent;
        event TouchEvent ClickedEvent;

        bool Dimmed { get; set; }
    }
}
