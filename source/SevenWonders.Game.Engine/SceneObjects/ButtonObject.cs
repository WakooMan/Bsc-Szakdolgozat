using SevenWonders.Common;
using SevenWonders.Game.Engine.InputHandling;
using SevenWonders.Game.Engine.SceneHandling;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace SevenWonders.Game.Engine.SceneObjects
{
    public class ButtonObject : SceneObject, IInteractiveObject, IEquatable<ButtonObject>
    {

        public event IInteractiveObject.TouchEvent ReleasedEvent = delegate { };
        public event IInteractiveObject.TouchEvent PressedEvent = delegate { };
        public event IInteractiveObject.TouchEvent MoveEvent = delegate { };
        public event IInteractiveObject.TouchEvent ClickedEvent = delegate { };

        public int BackgroundTextureId { get; set; }
        public TextProperties TextProperties { get; set; }

        public ButtonObject() : base()
        {
            TextProperties = new TextProperties();
        }

        public ButtonObject(ButtonObject other) : base(other)
        {
            BackgroundTextureId = other.BackgroundTextureId;
            TextProperties = new TextProperties(other.TextProperties);
        }

        public bool Equals(ButtonObject? other)
        {
            return base.Equals(other) && 
                   TextProperties.Equals(other?.TextProperties);
        }

        public override void Resize(Vector2 oldResolution, Vector2 newResolution)
        {
            base.Resize(oldResolution, newResolution);
            TextProperties.Resize(oldResolution, newResolution);
        }

        [ExcludeFromCodeCoverage]
        public override void Draw(SKCanvas canvas, TextureRegistry textureRegistry)
        {
            if (!Visible)
                return;

            if (BackgroundTextureId != -1)
            {
                Texture texture = textureRegistry.Get(BackgroundTextureId);
                if (Dimmed)
                {
                    texture.CustomColorFilter = SKColorFilter.CreateBlendMode(
                                        SKColors.Black.WithAlpha(120),
                                        SKBlendMode.SrcOver
                                    );
                }
                else if (texture.CustomColorFilter is not null)
                {
                    texture.CustomColorFilter = null;
                }

                texture.Draw(canvas, Position, Scale, Rotation, Width, Height);
            }

            if (!string.IsNullOrEmpty(TextProperties.Text))
            {
                var typeface = SKTypeface.FromFamilyName(TextProperties.Bold ? "CinzelBold" : "CinzelRegular",
                                                         TextProperties.Bold ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal,
                                                         SKFontStyleWidth.Normal,
                                                         SKFontStyleSlant.Upright);

                var matrix = SKMatrix.CreateTranslation(Position.X, Position.Y);
                matrix = matrix.PreConcat(SKMatrix.CreateRotationDegrees(Rotation));
                matrix = matrix.PreConcat(SKMatrix.CreateScale(Scale.X, Scale.Y));

                canvas.SetMatrix(matrix);

                using var font = new SKFont
                {
                    Typeface = typeface,
                    Size = TextProperties.FontSize,
                    Edging = SKFontEdging.Antialias
                };

                using var textPaint = new SKPaint
                {
                    Color = TextProperties.TextColor,
                    ColorFilter = Dimmed ? SKColorFilter.CreateBlendMode(
                                SKColors.Black.WithAlpha(120),
                                SKBlendMode.SrcOver
                            ) : null
                };

                float textY = font.Metrics.CapHeight / 2;

                canvas.DrawText(TextProperties.Text, 0, textY, SKTextAlign.Center, font, textPaint);
            }
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
