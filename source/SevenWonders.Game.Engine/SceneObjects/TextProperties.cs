using SkiaSharp;
using System.Numerics;
using System.Xml.Serialization;

namespace SevenWonders.Game.Engine.SceneObjects
{
    public class TextProperties
    {
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

        public TextProperties()
        {
            Text = string.Empty;
            Bold = false;
            FontSize = 24f;
            TextColor = SKColors.White;
        }

        public TextProperties(TextProperties other)
        {
            Text = new string(other.Text);
            Bold = other.Bold;
            FontSize = other.FontSize;
            TextColor = other.TextColor;
        }

        public void Resize(Vector2 oldResolution, Vector2 newResolution)
        {
            float xRatio = newResolution.X / oldResolution.X;
            float yRatio = newResolution.Y / oldResolution.Y;
            float matchFactor = 0.5f;
            float logWidth = MathF.Log2(xRatio);
            float logHeight = MathF.Log2(yRatio);
            float logWeightedAverage = logWidth * (1 - matchFactor) + logHeight * matchFactor;
            float finalScale = MathF.Pow(2, logWeightedAverage);

            FontSize = FontSize * finalScale;
        }
    }
}
