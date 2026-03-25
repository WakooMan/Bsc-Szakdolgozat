using Microsoft.Maui.Storage;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWonders.GameEngine
{
    public class TextureObject : SceneObject, IEquatable<TextureObject>
    {
        public int TextureId { get; set; }

        public TextureObject()
        {
            Name = string.Empty;
            TextureId = -1;
        }

        public TextureObject(TextureObject textureObj) : base(textureObj)
        {
            TextureId = textureObj.TextureId;
        }

        public bool Equals(TextureObject? other)
        {
            if (other is null)
            {
                return false;
            }

            return base.Equals(other) &&
                   TextureId.Equals(other.TextureId);
        }

        public override bool Equals(object? obj)
        {
            if (obj is TextureObject textureObject)
            {
                return Equals(textureObject);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^
                   TextureId.GetHashCode();
        }

        public void Draw(SKPaintSurfaceEventArgs eventArgs, TextureRegistry textureRegistry)
        {
            if (!Visible)
                return;

            textureRegistry.Get(TextureId).Draw(eventArgs, Position, Scale, Rotation, Width, Height);
        }
    }
}
