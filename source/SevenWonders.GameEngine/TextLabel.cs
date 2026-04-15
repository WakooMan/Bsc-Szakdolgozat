using SkiaSharp;
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
        public bool Bold { get; set; }
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
            Bold = false;
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
            Bold = other.Bold;
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
            float matchFactor = 0.5f;
            float logWidth = MathF.Log2(xRatio);
            float logHeight = MathF.Log2(yRatio);
            float logWeightedAverage = logWidth * (1 - matchFactor) + logHeight * matchFactor;
            float finalScale = MathF.Pow(2, logWeightedAverage);

            FontSize = FontSize * finalScale;
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

            if (!string.IsNullOrEmpty(Text))
            {
                var typeface = SKTypeface.FromFamilyName(Bold ? "CinzelBold" : "CinzelRegular",
                                                         Bold ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal,
                                                         SKFontStyleWidth.Normal,
                                                         SKFontStyleSlant.Upright);

                using var font = new SKFont
                {
                    Typeface = typeface,
                    Size = FontSize,
                    Edging = SKFontEdging.Antialias
                };

                using var textPaint = new SKPaint
                {
                    Color = TextColor,
                    ColorFilter = Dimmed ? SKColorFilter.CreateBlendMode(
                                SKColors.Black.WithAlpha(120),
                                SKBlendMode.SrcOver
                            ) : null
                };

                float textY = font.Metrics.CapHeight / 2;

                canvas.DrawText(Text, 0, textY, SKTextAlign.Center, font, textPaint);
            }
        }

        public override SceneObject Clone()
        {
            return new TextLabel(this);
        }
    }
}
