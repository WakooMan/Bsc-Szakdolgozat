using SevenWonders.Common;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Xml.Serialization;

namespace SevenWonders.GameEngine
{
    public class ButtonObject : TextLabel, IEquatable<ButtonObject>
    {
        public delegate void TouchEvent(SKTouchEventArgs eventArgs);

        public event TouchEvent ReleasedEvent = delegate { };
        public event TouchEvent PressedEvent = delegate { };
        public event TouchEvent MoveEvent = delegate { };
        public event TouchEvent ClickedEvent = delegate { };

        public ButtonObject() : base()
        {
        }

        public ButtonObject(ButtonObject other) : base(other)
        {
        }

        public bool Equals(ButtonObject? other)
        {
            return base.Equals(other);
        }

        public void OnTouchReleased(SKTouchEventArgs eventArgs, GraphicsLayer graphicsLayer)
        {
            if (graphicsLayer.Visible && Visible && IsTouchInButton(eventArgs.Location.X, eventArgs.Location.Y))
            {
                GameLog.Info($"ButtonObject released with ID: {Id} and name: {Name}");
                ReleasedEvent(eventArgs);
            }
        }

        public void OnTouchPressed(SKTouchEventArgs eventArgs, GraphicsLayer graphicsLayer)
        {
            if (graphicsLayer.Visible && Visible && IsTouchInButton(eventArgs.Location.X, eventArgs.Location.Y))
            {
                GameLog.Info($"ButtonObject pressed with ID: {Id} and name: {Name}");
                PressedEvent(eventArgs);
            }
        }

        public void OnTouchMoved(SKTouchEventArgs eventArgs, GraphicsLayer graphicsLayer)
        {
            if (graphicsLayer.Visible && Visible && IsTouchInButton(eventArgs.Location.X, eventArgs.Location.Y))
            {
                MoveEvent(eventArgs);
            }
        }

        public void OnTouchClicked(SKTouchEventArgs eventArgs, GraphicsLayer graphicsLayer)
        {
            if (graphicsLayer.Visible && Visible && IsTouchInButton(eventArgs.Location.X, eventArgs.Location.Y))
            {
                GameLog.Info($"ButtonObject clicked with ID: {Id} and name: {Name}");
                ClickedEvent(eventArgs);
            }
        }

        private bool IsTouchInButton(float x, float y)
        {
            float actualHalfWidth = Math.Abs(Width * Scale.X / 2);
            float actualHalfHeight = Math.Abs(Height * Scale.Y / 2);
            return x >= Position.X - actualHalfWidth &&
                   x <= Position.X + actualHalfWidth &&
                   y >= Position.Y - actualHalfHeight &&
                   y <= Position.Y + actualHalfHeight;
        }
    }
}
