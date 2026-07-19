using SkiaSharp;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Xml.Serialization;

namespace SevenWonders.Game.Engine.SceneObjects
{
    public class Texture : IEquatable<Texture>, IDisposable
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

            int cacheWidth = (int)Math.Round(width);
            int cacheHeight = (int)Math.Round(height);
            if (cacheWidth > 0 && cacheHeight > 0 && cacheWidth < 256 && cacheHeight < 256)
            {
                float scaleFactor = 256f / Math.Min(cacheWidth, cacheHeight);
                cacheWidth = (int)Math.Round(cacheWidth * scaleFactor);
                cacheHeight = (int)Math.Round(cacheHeight * scaleFactor);
            }
            var key = (Width: cacheWidth, Height: cacheHeight);
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
            var srcRect = new SKRect(0, 0, cachedImage.Width, cachedImage.Height);
            var destRect = new SKRect(-width / 2, -height / 2, width / 2, height / 2);
            canvas.DrawImage(cachedImage, destRect, sampling, m_defaultPaint);
        }

        [ExcludeFromCodeCoverage]
        public void DrawPart(SKCanvas canvas, Vector2 position, Vector2 scale, float rotation, int left, int top, int right, int bottom, float width, float height)
        {
            if (m_bitmap is null)
                return;

            var actualSizes = (Width: right - left, Height: bottom - top);
            float scaleX = width / actualSizes.Width;
            float scaleY = height / actualSizes.Height;
            int cacheWidth = (int)Math.Round(OriginalWidth * scaleX);
            int cacheHeight = (int)Math.Round(OriginalHeight * scaleY);
            if (cacheWidth > 0 && cacheHeight > 0 && cacheWidth < 256 && cacheHeight < 256)
            {
                float scaleFactor = 256f / Math.Min(cacheWidth, cacheHeight);
                cacheWidth = (int)Math.Round(cacheWidth * scaleFactor);
                cacheHeight = (int)Math.Round(cacheHeight * scaleFactor);
            }
            var key = (Width: cacheWidth, Height: cacheHeight);
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

        public void Dispose()
        {
            if(m_bitmap is not null)
            {
                m_bitmap.Dispose();
                m_bitmap = null;
            }
            if(m_customColorFilter is not null)
            {
                m_customColorFilter.Dispose();
                m_customColorFilter = null;
            }
            if(m_defaultPaint is not null)
            {
                m_defaultPaint.Dispose();
                m_defaultPaint = null;
            }
            foreach (var pair in m_sizeCache)
            {
                pair.Value.Dispose();
            }
            m_sizeCache.Clear();
        }

        public void ClearCache()
        {
            foreach (var pair in m_sizeCache)
            {
                pair.Value.Dispose();
            }

            m_sizeCache.Clear();
        }

        private SKBitmap? m_bitmap;
        [XmlIgnore]
        private readonly Dictionary<(int Width, int Height), SKImage> m_sizeCache;
        private SKColorFilter? m_customColorFilter;
        private SKPaint? m_defaultPaint;
    }
}
