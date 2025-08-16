using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWonders.GameEngine
{
    public class Texture : IEquatable<Texture>
    {
        public Vector2 Position { get; set; }
        public Vector2 Rotation { get; set; }
        public Vector2 Scale { get; set; }
        public bool Visible { get; set; }
        public float OriginalWidth { get; set; }
        public float OriginalHeight { get; set; }
        public SKColor Color { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public int TextureId { get; set; }
        public int Id { get; set; }

        public Texture()
        {
            Name = string.Empty;
            FileName = string.Empty;
        }

        public Texture(Texture texture)
        {
            Name = texture.Name;
            FileName = texture.FileName;
            Width = texture.Width;
            Height = texture.Height;
            OriginalWidth = texture.OriginalWidth;
            OriginalHeight = texture.OriginalHeight;
            Color = texture.Color;
            TextureId = texture.TextureId;
            Id = texture.Id;
            Position = texture.Position;
            Rotation = texture.Rotation;
            Scale = texture.Scale;
            Visible = texture.Visible;
        }

        public bool Equals(Texture? other)
        {
            if (other is null)
            {
                return false;
            }

            return Name.Equals(other.Name) &&
                   FileName.Equals(other.FileName) &&
                   Width.Equals(other.Width) &&
                   Height.Equals(other.Height) &&
                   OriginalHeight.Equals(other.OriginalHeight) &&
                   OriginalWidth.Equals(other.OriginalWidth) &&
                   Color.Equals(other.Color) &&
                   TextureId.Equals(other.TextureId) &&
                   Id.Equals(other.Id) &&
                   Position.Equals(other.Position) &&
                   Rotation.Equals(other.Rotation) &&
                   Scale.Equals(other.Scale) &&
                   Visible.Equals(other.Visible);
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
            return Name.GetHashCode() +
                   FileName.GetHashCode() +
                   Width.GetHashCode() +
                   Height.GetHashCode() +
                   OriginalHeight.GetHashCode() +
                   OriginalWidth.GetHashCode() +
                   Color.GetHashCode() +
                   TextureId.GetHashCode() +
                   Id.GetHashCode() +
                   Position.GetHashCode() +
                   Rotation.GetHashCode() +
                   Scale.GetHashCode() +
                   Visible.GetHashCode();
        }

        public void LoadTexture()
        {
            using var stream = File.OpenRead(FileName);
            m_bitmap = SKBitmap.Decode(stream);
            OriginalWidth = m_bitmap.Width;
            OriginalHeight = m_bitmap.Height;
            if (Width == 0) Width = OriginalWidth;
            if (Height == 0) Height = OriginalHeight;
        }

        public void Draw(SKPaintSurfaceEventArgs eventArgs)
        {
            if (!Visible || m_bitmap == null)
                return;

            var canvas = eventArgs.Surface.Canvas;

            using var paint = new SKPaint
            {
                IsAntialias = true,
                ColorFilter = SKColorFilter.CreateBlendMode(Color, SKBlendMode.Modulate)
            };

            canvas.Save();
            canvas.Translate(Position.X, Position.Y);
            canvas.RotateDegrees(Rotation.X, Width / 2f, Height / 2f);
            canvas.Scale(Scale.X, Scale.Y);
            var destRect = new SKRect(0, 0, Width, Height);
            canvas.DrawBitmap(m_bitmap, destRect);
            canvas.Restore();
        }

        private SKBitmap m_bitmap;
    }
}
