using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWonders.GameEngine
{
    public class SpriteFrame: IEquatable<SpriteFrame>
    {
        public Texture Frame { get; set; }
        public string Name { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public BoundingBox BBoxOriginal { get; set; }
        public BoundingBox BBoxTransformed { get; set; }

        public SpriteFrame()
        {
            Name = string.Empty;
            Frame = new Texture();
            BBoxOriginal = new BoundingBox();
            BBoxTransformed = new BoundingBox();
        }

        public SpriteFrame(SpriteFrame spriteFrame)
        {
            Name = spriteFrame.Name;
            Frame = new Texture(spriteFrame.Frame);
            BBoxOriginal = new BoundingBox(spriteFrame.BBoxOriginal);
            BBoxTransformed = new BoundingBox(spriteFrame.BBoxTransformed);
        }

        public bool Equals(SpriteFrame? other)
        {
            if (other is null)
            {
                return false;
            }

            return Name.Equals(other.Name) &&
                   Frame.Equals(other.Frame) &&
                   BBoxOriginal.Equals(other.BBoxOriginal) &&
                   BBoxTransformed.Equals(other.BBoxTransformed);
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
            return Name.GetHashCode() ^
                   Frame.GetHashCode() ^
                   BBoxOriginal.GetHashCode() ^
                   BBoxTransformed.GetHashCode();
        }

        public void Draw(SKPaintSurfaceEventArgs eventArgs, Vector2 position, Vector2 scale, float rotation)
        {
            if(!Frame.Visible)
                return;

            Frame.DrawPart(eventArgs, position, scale, rotation, Left, Top, Right, Bottom);
        }

        public void LoadTexture(string sceneFolder)
        {
            Frame.LoadTexture(sceneFolder);
        }

    }
}