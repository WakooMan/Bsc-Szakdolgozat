using SevenWonders.Game.Engine.SceneHandling;
using SkiaSharp;

namespace SevenWonders.Game.Engine.SceneObjects
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

        public override void Draw(SKCanvas canvas, TextureRegistry textureRegistry)
        {
            if (!Visible)
                return;

            textureRegistry.Get(TextureId).Draw(canvas, Position, Scale, Rotation, Width, Height);
        }

        public override SceneObject Clone()
        {
            return new TextureObject(this);
        }
    }
}
