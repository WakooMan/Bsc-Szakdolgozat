using Microsoft.Maui.Storage;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWonders.GameEngine
{
    public class TextureObject : IEquatable<TextureObject>
    {
        public string Name { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public int Id { get; set; }
        public bool Visible { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; }
        public Texture Texture { get; set; }

        public TextureObject()
        {
            Name = string.Empty;
            Texture = new Texture();
        }

        public TextureObject(TextureObject textureObj)
        {
            Name = new string(textureObj.Name);
            Width = textureObj.Width;
            Height = textureObj.Height;
            Id = textureObj.Id;
            Position = textureObj.Position;
            Rotation = textureObj.Rotation;
            Scale = textureObj.Scale;
            Visible = textureObj.Visible;
            Texture = new Texture(textureObj.Texture);
        }

        public bool Equals(TextureObject? other)
        {
            if (other is null)
            {
                return false;
            }

            return Name.Equals(other.Name) &&
                   Width.Equals(other.Width) &&
                   Height.Equals(other.Height) &&
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
            return Name.GetHashCode() ^
                   Width.GetHashCode() ^
                   Height.GetHashCode() ^
                   Id.GetHashCode() ^
                   Position.GetHashCode() ^
                   Rotation.GetHashCode() ^
                   Scale.GetHashCode() ^
                   Visible.GetHashCode();
        }

        public void Resize(Vector2 oldResolution, Vector2 newResolution)
        {
            float XRatio = newResolution.X / oldResolution.X;
            float YRatio = newResolution.Y / oldResolution.Y;
            Position = new Vector2(Position.X * XRatio, Position.Y * YRatio);
            Width = Width * XRatio;
            Height = Height * YRatio;
        }

        public void LoadTexture(string sceneFolder)
        {
            Texture.LoadTexture(sceneFolder);
            if (Width == 0) Width = Texture.OriginalWidth;
            if (Height == 0) Height = Texture.OriginalHeight;
        }

        public void Draw(SKPaintSurfaceEventArgs eventArgs)
        {
            if (!Visible)
                return;

            Texture.Draw(eventArgs, Position, Scale, Rotation, Width, Height);
        }
    }
}
