using SevenWonders.Game.Engine.SceneHandling;
using SkiaSharp;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Xml.Serialization;

namespace SevenWonders.Game.Engine.SceneObjects
{
    [XmlInclude(typeof(ButtonObject))]
    public class TextLabel : SceneObject, IEquatable<TextLabel>
    {
        public TextProperties TextProperties { get; set; }

        public TextLabel()
        {
            Name = string.Empty;
            Scale = new Vector2(1, 1);
            TextProperties = new TextProperties();
        }

        public TextLabel(TextLabel other) : base(other)
        {
            TextProperties = new TextProperties(other.TextProperties);
        }

        public bool Equals(TextLabel? other)
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

        public override SceneObject Clone()
        {
            return new TextLabel(this);
        }

        public override bool IsStatic()
        {
            return false;
        }
    }
}
