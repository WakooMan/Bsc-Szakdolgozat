using SkiaSharp;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Xml.Serialization;

namespace SevenWonders.GameEngine
{
    public class Texture : IEquatable<Texture>
    {
        public float OriginalWidth { get; set; }
        public float OriginalHeight { get; set; }
        public string FileName { get; set; }
        public int Id { get; set; }

        [XmlIgnore]
        public SKColorFilter? CustomColorFilter
        {
            get => m_customColorFilter;
            set
            {
                if (m_customColorFilter == value) return;
                m_customColorFilter?.Dispose();
                m_customColorFilter = value;
                m_defaultPaint?.Dispose();
                m_defaultPaint = null;
            }
        }

        public Texture()
        {
            FileName = string.Empty;
        }

        public Texture(Texture texture)
        {
            FileName = new string(texture.FileName);
            OriginalWidth = texture.OriginalWidth;
            OriginalHeight = texture.OriginalHeight;
            Id = texture.Id;
        }

        public bool Equals(Texture? other)
        {
            if (other is null)
            {
                return false;
            }

            return FileName.Equals(other.FileName) &&
                   OriginalHeight.Equals(other.OriginalHeight) &&
                   OriginalWidth.Equals(other.OriginalWidth) &&
                   Id.Equals(other.Id);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Texture texture)
            {
                return Equals(texture);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return FileName.GetHashCode() ^
                   OriginalHeight.GetHashCode() ^
                   OriginalWidth.GetHashCode() ^
                   Id.GetHashCode();
        }

        public void LoadTexture(string sceneFolder, GRContext gRContext)
        {
            using var stream = File.OpenRead(Path.Combine(sceneFolder, FileName));
            using var cpuImage = SKImage.FromEncodedData(stream);
            m_image = cpuImage.ToTextureImage(gRContext);
            OriginalWidth = m_image.Width;
            OriginalHeight = m_image.Height;
        }

        [ExcludeFromCodeCoverage]
        public void Draw(SKCanvas canvas, Vector2 position, Vector2 scale, float rotation, float width, float height)
        {
            if (m_image == null)
                return;

            m_defaultPaint ??= new SKPaint { IsAntialias = false, ColorFilter = m_customColorFilter };
            SKSamplingOptions samplingOptions = new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear);

            var matrix = SKMatrix.CreateTranslation(position.X, position.Y);
            matrix = matrix.PreConcat(SKMatrix.CreateRotationDegrees(rotation));
            matrix = matrix.PreConcat(SKMatrix.CreateScale(scale.X, scale.Y));

            canvas.SetMatrix(matrix);

            var destRect = new SKRect(-width / 2, -height / 2, width / 2, height / 2);
            canvas.DrawImage(m_image, destRect, samplingOptions, m_defaultPaint);
        }

        [ExcludeFromCodeCoverage]
        public void DrawPart(SKCanvas canvas, Vector2 position, Vector2 scale, float rotation, int left, int top, int right, int bottom, float width, float height)
        {
            if (m_image == null)
                return;

            m_defaultPaint ??= new SKPaint { IsAntialias = false, ColorFilter = m_customColorFilter };
            SKSamplingOptions samplingOptions = new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear);

            var matrix = SKMatrix.CreateTranslation(position.X, position.Y);
            matrix = matrix.PreConcat(SKMatrix.CreateRotationDegrees(rotation));
            matrix = matrix.PreConcat(SKMatrix.CreateScale(scale.X, scale.Y));

            canvas.SetMatrix(matrix);

            var srcRect = new SKRectI(left, top, right, bottom);
            var destRect = new SKRect(-width / 2, -height / 2, width / 2, height / 2);
            canvas.DrawImage(m_image, srcRect, destRect, m_defaultPaint);
        }

        private SKImage? m_image;
        private SKColorFilter? m_customColorFilter;
        private SKPaint? m_defaultPaint;
    }
}
