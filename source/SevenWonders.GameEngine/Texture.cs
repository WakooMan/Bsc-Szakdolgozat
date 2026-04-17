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
            m_sizeCache = new Dictionary<(int Width, int Height), SKImage>();
        }

        public Texture(Texture texture)
        {
            FileName = new string(texture.FileName);
            OriginalWidth = texture.OriginalWidth;
            OriginalHeight = texture.OriginalHeight;
            Id = texture.Id;
            m_sizeCache = new Dictionary<(int Width, int Height), SKImage>();
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

        public void LoadTexture(string sceneFolder)
        {
            using var stream = File.OpenRead(Path.Combine(sceneFolder, FileName));
            m_bitmap = SKBitmap.Decode(stream);
            OriginalWidth = m_bitmap.Width;
            OriginalHeight = m_bitmap.Height;
        }

        [ExcludeFromCodeCoverage]
        public void Draw(SKCanvas canvas, Vector2 position, Vector2 scale, float rotation, float width, float height)
        {
            if (m_bitmap is null)
                return;

            var key = (Width: (int)Math.Round(width), Height: (int)Math.Round(height));
            SKSamplingOptions highQualitySampling = new SKSamplingOptions(SKCubicResampler.Mitchell);
            if (!m_sizeCache.TryGetValue(key, out SKImage? cachedImage))
            {
                var info = new SKImageInfo(key.Width, key.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
                using (SKBitmap resized = m_bitmap.Resize(info, highQualitySampling))
                {
                    cachedImage = SKImage.FromBitmap(resized);
                    m_sizeCache[key] = cachedImage;
                }
            }

            m_defaultPaint ??= new SKPaint { IsAntialias = true, ColorFilter = m_customColorFilter };

            var matrix = SKMatrix.CreateTranslation(position.X, position.Y);
            matrix = matrix.PreConcat(SKMatrix.CreateRotationDegrees(rotation));
            matrix = matrix.PreConcat(SKMatrix.CreateScale(scale.X, scale.Y));

            canvas.SetMatrix(matrix);

            SKSamplingOptions sampling = new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear);
            var destRect = new SKRect(-width / 2, -height / 2, width / 2, height / 2);
            canvas.DrawImage(cachedImage, destRect, sampling, m_defaultPaint);
        }

        [ExcludeFromCodeCoverage]
        public void DrawPart(SKCanvas canvas, Vector2 position, Vector2 scale, float rotation, int left, int top, int right, int bottom, float width, float height)
        {
            if (m_bitmap is null)
                return;

            

            var key = (Width: (int)Math.Round(width), Height: (int)Math.Round(height));
            SKSamplingOptions highQualitySampling = new SKSamplingOptions(SKCubicResampler.Mitchell);
            if (!m_sizeCache.TryGetValue(key, out SKImage? cachedImage))
            {
                var info = new SKImageInfo(key.Width, key.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
                using (SKBitmap resized = m_bitmap.Resize(info, highQualitySampling))
                {
                    cachedImage = SKImage.FromBitmap(resized);
                    m_sizeCache[key] = cachedImage;
                }
            }

            float scaleX = key.Width / OriginalWidth;
            float scaleY = key.Height / OriginalHeight;

            var srcRect = new SKRect(
                left * scaleX,
                top * scaleY,
                right * scaleX,
                bottom * scaleY
            );

            m_defaultPaint ??= new SKPaint { IsAntialias = true, ColorFilter = m_customColorFilter };

            var matrix = SKMatrix.CreateTranslation(position.X, position.Y);
            matrix = matrix.PreConcat(SKMatrix.CreateRotationDegrees(rotation));
            matrix = matrix.PreConcat(SKMatrix.CreateScale(scale.X, scale.Y));
            SKSamplingOptions sampling = new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear);

            canvas.SetMatrix(matrix);
            var destRect = new SKRect(-width / 2, -height / 2, width / 2, height / 2);
            canvas.DrawImage(cachedImage, srcRect, destRect, sampling, m_defaultPaint);
        }

        private SKBitmap? m_bitmap;
        [XmlIgnore]
        private readonly Dictionary<(int Width, int Height), SKImage> m_sizeCache;
        private SKColorFilter? m_customColorFilter;
        private SKPaint? m_defaultPaint;
    }
}
