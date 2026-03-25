using SevenWonders.Common;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Xml.Serialization;

namespace SevenWonders.GameEngine
{
    public class TextLabel : SceneObject, IEquatable<TextLabel>
    {
        public int BackgroundTextureId { get; set; }
        public string Text { get; set; }
        public float FontSize { get; set; }

        [XmlIgnore]
        public SKColor TextColor { get; set; }

        public string TextColorHex
        {
            get => TextColor.ToString();
            set => TextColor = SKColor.Parse(value);
        }

        public TextLabel()
        {
            Name = string.Empty;
            Text = string.Empty;
            Scale = new Vector2(1, 1);
            FontSize = 24f;
            TextColor = SKColors.White;
        }

        public TextLabel(TextLabel other) : base(other)
        {
            BackgroundTextureId = other.BackgroundTextureId;
            Text = new string(other.Text);
            FontSize = other.FontSize;
            TextColor = other.TextColor;
        }

        public bool Equals(TextLabel? other)
        {
            if (other is null)
            {
                return false;
            }

            return base.Equals(other) &&
                   BackgroundTextureId.Equals(other.BackgroundTextureId) &&
                   Text.Equals(other.Text) &&
                   FontSize.Equals(other.FontSize) &&
                   TextColor.Equals(other.TextColor);
        }

        public override bool Equals(object? obj)
        {
            if (obj is TextLabel textLabel)
            {
                return Equals(textLabel);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^
                   BackgroundTextureId.GetHashCode() ^
                   Text.GetHashCode() ^
                   FontSize.GetHashCode() ^
                   TextColor.GetHashCode();
        }

        public override void Resize(Vector2 oldResolution, Vector2 newResolution)
        {
            base.Resize(oldResolution, newResolution);
            float xRatio = newResolution.X / oldResolution.X;
            float yRatio = newResolution.Y / oldResolution.Y;
            FontSize = FontSize * Math.Min(xRatio, yRatio);
        }

        [ExcludeFromCodeCoverage]
        public void Draw(SKPaintSurfaceEventArgs eventArgs, TextureRegistry textureRegistry)
        {
            if (!Visible)
                return;

            var canvas = eventArgs.Surface.Canvas;

            canvas.Save();
            canvas.Translate(Position.X, Position.Y);
            canvas.RotateDegrees(Rotation);
            canvas.Scale(Scale.X, Scale.Y);

            // Draw background image
            textureRegistry.Get(BackgroundTextureId).Draw(eventArgs, Vector2.Zero, Vector2.One, 0, Width, Height);

            // Draw text on top
            if (!string.IsNullOrEmpty(Text))
            {
                using var textPaint = new SKPaint
                {
                    IsAntialias = true,
                    Color = TextColor,
                    TextSize = FontSize,
                    TextAlign = SKTextAlign.Center,
                    IsStroke = false
                };

                float textY = textPaint.FontMetrics.CapHeight / 2;
                canvas.DrawText(Text, 0, textY, textPaint);
            }

            canvas.Restore();
        }
    }
}
