using SevenWonders.Common;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Xml.Serialization;

namespace SevenWonders.GameEngine
{
    [XmlInclude(typeof(ButtonObject))]
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
            return base.Equals(other);
        }

        public override void Resize(Vector2 oldResolution, Vector2 newResolution)
        {
            base.Resize(oldResolution, newResolution);
            float xRatio = newResolution.X / oldResolution.X;
            float yRatio = newResolution.Y / oldResolution.Y;
            FontSize = FontSize * Math.Min(xRatio, yRatio);
        }

        [ExcludeFromCodeCoverage]
        public override void Draw(SKPaintSurfaceEventArgs eventArgs, TextureRegistry textureRegistry)
        {
            if (!Visible)
                return;

            var canvas = eventArgs.Surface.Canvas;

            canvas.Save();
            canvas.Translate(Position.X, Position.Y);
            canvas.RotateDegrees(Rotation);
            canvas.Scale(Scale.X, Scale.Y);

            Texture texture = textureRegistry.Get(BackgroundTextureId);
            if (Dimmed)
            {
                texture.CustomColorFilter = SKColorFilter.CreateBlendMode(
                                    SKColors.Black.WithAlpha(120),
                                    SKBlendMode.SrcOver
                                );
            }
            else if(texture.CustomColorFilter is not null)
            {
                texture.CustomColorFilter = null;
            }

            texture.Draw(eventArgs, Vector2.Zero, Vector2.One, 0, Width, Height);

            if (!string.IsNullOrEmpty(Text))
            {
                using var textPaint = new SKPaint
                {
                    IsAntialias = true,
                    Color = TextColor,
                    TextSize = FontSize,
                    TextAlign = SKTextAlign.Center,
                    IsStroke = false,
                    ColorFilter = Dimmed ? SKColorFilter.CreateBlendMode(
                                    SKColors.Black.WithAlpha(120),
                                    SKBlendMode.SrcOver
                                ) : null
                };

                float textY = textPaint.FontMetrics.CapHeight / 2;
                canvas.DrawText(Text, 0, textY, textPaint);
            }

            canvas.Restore();
        }

        public override SceneObject Clone()
        {
            return new TextLabel(this);
        }
    }
}
