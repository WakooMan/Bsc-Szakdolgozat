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
            return base.Equals(other);
        }

        public void Draw(SKPaintSurfaceEventArgs eventArgs, TextureRegistry textureRegistry)
        {
            if (!Visible)
                return;

            textureRegistry.Get(TextureId).Draw(eventArgs, Position, Scale, Rotation, Width, Height);
        }
    }
}
