using SevenWonders.Common;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace SevenWonders.GameEngine
{
    public class GameObject : SceneObject, IEquatable<GameObject>
    {
        public delegate void TouchEvent(SKTouchEventArgs eventArgs);

        public event TouchEvent ReleasedEvent = delegate { };
        public event TouchEvent PressedEvent = delegate { };
        public event TouchEvent MoveEvent = delegate { };
        public event TouchEvent ClickedEvent = delegate { };

        public Vector2 Direction { get; set;}
        public Vector2 VisualSize { get; set; }
        public Vector2 FlipMultiplier { get; set; }
        public List<Sprite> Animations { get; set; }
        public float Speed { get; set; }
        public bool Collidable { get; set; }
        public bool InFrustum { get; set; }
        public int CurrentAnim { get; set; }
        public int NumberOfFrames { get; set; }
        public bool Highlighted { get; set; }

        public GameObject()
        {
            Name = string.Empty;
            Animations = new List<Sprite>();
            CurrentAnim = 0;
            Highlighted = false;
            VisualSize = new Vector2(1.0f, 1.0f);
            FlipMultiplier = new Vector2(1.0f, 1.0f);
        }

        public GameObject(GameObject gameObject) : base(gameObject)
        {
            Direction = gameObject.Direction;
            VisualSize = gameObject.VisualSize;
            FlipMultiplier = gameObject.FlipMultiplier;
            Collidable = gameObject.Collidable;
            InFrustum = gameObject.InFrustum;
            Animations = gameObject.Animations.Select(sprite => new Sprite(sprite)).ToList();
            CurrentAnim = gameObject.CurrentAnim;
            NumberOfFrames = gameObject.NumberOfFrames;
            Speed = gameObject.Speed;
            Highlighted = gameObject.Highlighted;
        }

        public bool Equals(GameObject? other)
        {
            if (other is null)
            {
                return false;
            }

            return base.Equals(other) &&
                   Direction.Equals(other.Direction) &&
                   VisualSize.Equals(other.VisualSize) &&
                   FlipMultiplier.Equals(other.FlipMultiplier) &&
                   Collidable.Equals(other.Collidable) &&
                   InFrustum.Equals(other.InFrustum) &&
                   Animations.SequenceEqual(other.Animations) &&
                   CurrentAnim.Equals(other.CurrentAnim) &&
                   NumberOfFrames.Equals(other.NumberOfFrames) &&
                   Speed.Equals(other.Speed) &&
                   Highlighted.Equals(other.Highlighted);
        }

        public override bool Equals(object? obj)
        {
            if (obj is GameObject gameObject)
            {
                return Equals(gameObject);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode() ^
                   Direction.GetHashCode() ^
                   VisualSize.GetHashCode() ^
                   FlipMultiplier.GetHashCode() ^
                   Collidable.GetHashCode() ^
                   InFrustum.GetHashCode() ^
                   CurrentAnim.GetHashCode() ^
                   NumberOfFrames.GetHashCode() ^
                   Speed.GetHashCode() ^
                   Highlighted.GetHashCode();
            Animations.ForEach(anim => hashCode = hashCode ^ anim.GetHashCode());
            return hashCode;
        }

        public override void Resize(Vector2 oldResolution, Vector2 newResolution)
        {
            base.Resize(oldResolution, newResolution);
        }

        [ExcludeFromCodeCoverage]
        public void Draw(SKPaintSurfaceEventArgs eventArgs, TextureRegistry textureRegistry)
        {
            if (!Visible || Animations.Count <= 0)
            { 
                return;
            }

            var effectiveScale = new Vector2(VisualSize.X * FlipMultiplier.X, VisualSize.Y * FlipMultiplier.Y);

            var canvas = eventArgs.Surface.Canvas;

            if (Highlighted)
            {
                canvas.Save();

                canvas.Translate(Position.X, Position.Y);
                
                canvas.RotateDegrees(Rotation);
                canvas.Scale(effectiveScale.X, effectiveScale.Y);

                using (var highlightPaint = new SKPaint())
                {
                    highlightPaint.IsAntialias = true;
                    highlightPaint.Style = SKPaintStyle.Stroke;
                    highlightPaint.StrokeWidth = 8;
                    highlightPaint.Color = SKColors.Gold.WithAlpha(200);

                    highlightPaint.MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, 10);

                    var rect = new SKRect(-Width / 2, -Height / 2, Width / 2, Height / 2);
                    canvas.DrawRoundRect(rect, 15, 15, highlightPaint);
                }

                canvas.Restore();
            }

            Animations[CurrentAnim].Draw(eventArgs, Position, effectiveScale, Rotation, Width, Height, textureRegistry);
        }

        public void OnTouchReleased(SKTouchEventArgs eventArgs, GraphicsLayer graphicsLayer)
        {
            if (graphicsLayer.Visible && Visible && IsTouchInGameObject(eventArgs.Location.X, eventArgs.Location.Y))
            {
                GameLog.Info($"GameObject released with ID: {Id} and name: {Name}");
                ReleasedEvent(eventArgs);
            }
        }

        public void OnTouchPressed(SKTouchEventArgs eventArgs, GraphicsLayer graphicsLayer)
        {
            if (graphicsLayer.Visible && Visible && IsTouchInGameObject(eventArgs.Location.X, eventArgs.Location.Y))
            {
                GameLog.Info($"GameObject pressed with ID: {Id} and name: {Name}");
                PressedEvent(eventArgs);
            }
        }

        public void OnTouchMoved(SKTouchEventArgs eventArgs, GraphicsLayer graphicsLayer)
        {
            if (graphicsLayer.Visible && Visible && IsTouchInGameObject(eventArgs.Location.X, eventArgs.Location.Y))
            {
                MoveEvent(eventArgs);
            }
        }

        public void OnTouchClicked(SKTouchEventArgs eventArgs, GraphicsLayer graphicsLayer)
        {
            if (graphicsLayer.Visible && Visible && IsTouchInGameObject(eventArgs.Location.X, eventArgs.Location.Y))
            {
                GameLog.Info($"GameObject clicked with ID: {Id} and name: {Name}");
                ClickedEvent(eventArgs);
            }
        }

        private bool IsTouchInGameObject(float x, float y)
        {
            float actualHalfWidth = Math.Abs(Width * VisualSize.X * FlipMultiplier.X / 2);
            float actualHalfHeight = Math.Abs(Height * VisualSize.Y * FlipMultiplier.Y / 2);
           return x >= Position.X - actualHalfWidth &&
           x <= Position.X + actualHalfWidth &&
           y >= Position.Y - actualHalfHeight &&
           y <= Position.Y + actualHalfHeight;
        }
    }
}
