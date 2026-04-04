using SkiaSharp;
using SkiaSharp.Views.Maui;
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

        public void LoadTexture(string sceneFolder)
        {
            using var stream = File.OpenRead(Path.Combine(sceneFolder, FileName));
            m_bitmap = SKBitmap.Decode(stream);
            OriginalWidth = m_bitmap.Width;
            OriginalHeight = m_bitmap.Height;
        }

        [ExcludeFromCodeCoverage]
        public void Draw(SKPaintSurfaceEventArgs eventArgs, Vector2 position, Vector2 scale, float rotation, float width, float height)
        {
            if (m_bitmap == null)
                return;

            m_defaultPaint ??= new SKPaint { IsAntialias = true, ColorFilter = m_customColorFilter };

            var canvas = eventArgs.Surface.Canvas;
            canvas.Save();
            canvas.Translate(position.X, position.Y);
            canvas.RotateDegrees(rotation);
            canvas.Scale(scale.X, scale.Y);
            var destRect = new SKRect(-width / 2, -height / 2, width / 2, height / 2);
            canvas.DrawBitmap(m_bitmap, destRect, m_defaultPaint);
            canvas.Restore();
        }

        [ExcludeFromCodeCoverage]
        public void DrawPart(SKPaintSurfaceEventArgs eventArgs, Vector2 position, Vector2 scale, float rotation, int left, int top, int right, int bottom, float width, float height)
        {
            if (m_bitmap == null)
                return;

            m_defaultPaint ??= new SKPaint { IsAntialias = true, ColorFilter = m_customColorFilter };

            var canvas = eventArgs.Surface.Canvas;
            canvas.Save();
            canvas.Translate(position.X, position.Y);
            canvas.RotateDegrees(rotation);
            canvas.Scale(scale.X, scale.Y);
            var srcRect = new SKRectI(left, top, right, bottom);
            var destRect = new SKRect(-width / 2, -height / 2, width / 2, height / 2);
            canvas.DrawBitmap(m_bitmap, srcRect, destRect, m_defaultPaint);
            canvas.Restore();
        }

        private SKBitmap? m_bitmap;
        private SKColorFilter? m_customColorFilter;
        private SKPaint? m_defaultPaint;
    }
}
