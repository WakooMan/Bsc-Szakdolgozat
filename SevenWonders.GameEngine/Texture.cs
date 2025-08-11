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

        public void Draw(SKPaintSurfaceEventArgs eventArgs)
        {

        }
    }
}
