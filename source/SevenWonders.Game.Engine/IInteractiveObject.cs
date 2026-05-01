using SkiaSharp.Views.Maui;

namespace SevenWonders.Game.Engine
{
    public interface IInteractiveObject
    {
        delegate void TouchEvent(IInteractiveObject interactiveObject, SKTouchEventArgs eventArgs);

        event TouchEvent ReleasedEvent;
        event TouchEvent PressedEvent;
        event TouchEvent MoveEvent;
        event TouchEvent ClickedEvent;

        bool Dimmed { get; set; }

        string Name { get; }

        int Id { get; }

        void OnTouchPressed(SKTouchEventArgs eventArgs, GraphicsLayer graphicsLayer);
        void OnTouchReleased(SKTouchEventArgs eventArgs, GraphicsLayer graphicsLayer);
        void OnTouchMoved(SKTouchEventArgs eventArgs, GraphicsLayer graphicsLayer);
        void OnTouchClicked(SKTouchEventArgs eventArgs, GraphicsLayer graphicsLayer);
    }
}
