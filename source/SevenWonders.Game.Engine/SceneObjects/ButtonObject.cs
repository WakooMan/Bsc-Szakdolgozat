using SevenWonders.Common;
using SevenWonders.Game.Engine.InputHandling;
using SevenWonders.Game.Engine.SceneHandling;
using SkiaSharp.Views.Maui;

namespace SevenWonders.Game.Engine.SceneObjects
{
    public class ButtonObject : TextLabel, IInteractiveObject, IEquatable<ButtonObject>
    {
        public event IInteractiveObject.TouchEvent ReleasedEvent = delegate { };
        public event IInteractiveObject.TouchEvent PressedEvent = delegate { };
        public event IInteractiveObject.TouchEvent MoveEvent = delegate { };
        public event IInteractiveObject.TouchEvent ClickedEvent = delegate { };

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
                ReleasedEvent(this, eventArgs);
            }
        }

        public void OnTouchPressed(SKTouchEventArgs eventArgs, GraphicsLayer graphicsLayer)
        {
            if (graphicsLayer.Visible && Visible && IsTouchInButton(eventArgs.Location.X, eventArgs.Location.Y))
            {
                GameLog.Info($"ButtonObject pressed with ID: {Id} and name: {Name}");
                PressedEvent(this, eventArgs);
            }
        }

        public void OnTouchMoved(SKTouchEventArgs eventArgs, GraphicsLayer graphicsLayer)
        {
            if (graphicsLayer.Visible && Visible && IsTouchInButton(eventArgs.Location.X, eventArgs.Location.Y))
            {
                MoveEvent(this, eventArgs);
            }
        }

        public void OnTouchClicked(SKTouchEventArgs eventArgs, GraphicsLayer graphicsLayer)
        {
            if (graphicsLayer.Visible && Visible && IsTouchInButton(eventArgs.Location.X, eventArgs.Location.Y))
            {
                GameLog.Info($"ButtonObject clicked with ID: {Id} and name: {Name}");
                ClickedEvent(this ,eventArgs);
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

        public override SceneObject Clone()
        {
            return new ButtonObject(this);
        }

        public override bool IsStatic()
        {
            return true;
        }
    }
}
