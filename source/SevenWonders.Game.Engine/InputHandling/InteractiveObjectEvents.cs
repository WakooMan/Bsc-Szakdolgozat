using SkiaSharp.Views.Maui;

namespace SevenWonders.Game.Engine.InputHandling
{
    public class InteractiveObjectEvents
    {
        public Action<SKTouchEventArgs> TouchPressed { get; }
        public Action<SKTouchEventArgs> TouchReleased { get; }
        public Action<SKTouchEventArgs> TouchClicked { get; }
        public Action<SKTouchEventArgs> TouchMoved { get; }

        public InteractiveObjectEvents(Action<SKTouchEventArgs> touchPressed, 
                                Action<SKTouchEventArgs> touchReleased, 
                                Action<SKTouchEventArgs> touchClicked, 
                                Action<SKTouchEventArgs> touchMoved)
        {
            TouchPressed = touchPressed;
            TouchReleased = touchReleased;
            TouchClicked = touchClicked;
            TouchMoved = touchMoved;
        }
    }
}
