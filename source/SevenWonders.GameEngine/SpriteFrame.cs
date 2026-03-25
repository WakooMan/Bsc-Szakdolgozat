using SkiaSharp.Views.Maui;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace SevenWonders.GameEngine
{
    public class SpriteFrame: IEquatable<SpriteFrame>
    {
        public int TextureId { get; set; }
        public string Name { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        public SpriteFrame()
        {
            Name = string.Empty;
        }

        public SpriteFrame(SpriteFrame spriteFrame)
        {
            Name = new string(spriteFrame.Name);
            TextureId = spriteFrame.TextureId;
            Left = spriteFrame.Left;
            Top = spriteFrame.Top;
            Right = spriteFrame.Right;
            Bottom = spriteFrame.Bottom;
        }

        public bool Equals(SpriteFrame? other)
        {
            if (other is null)
            {
                return false;
            }

            return Name.Equals(other.Name) &&
                   TextureId.Equals(other.TextureId) &&
                   Left.Equals(other.Left) &&
                   Top.Equals(other.Top) &&
                   Right.Equals(other.Right) &&
                   Bottom.Equals(other.Bottom);
        }

        public override bool Equals(object? obj)
        {
            if (obj is SpriteFrame spriteFrame)
            {
                return Equals(spriteFrame);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = Name.GetHashCode() ^
                   TextureId.GetHashCode() ^
                   Top.GetHashCode() ^
                   Left.GetHashCode() ^
                   Right.GetHashCode() ^
                   Bottom.GetHashCode();
            return hashCode;
        }

        [ExcludeFromCodeCoverage]
        public void Draw(SKPaintSurfaceEventArgs eventArgs, Vector2 position, Vector2 scale, float rotation, float width, float height, TextureRegistry textureRegistry)
        {
            textureRegistry.Get(TextureId).DrawPart(eventArgs, position, scale, rotation, Left, Top, Right, Bottom, width, height);
        }
    }
}