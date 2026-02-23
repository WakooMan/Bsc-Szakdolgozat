using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWonders.GameEngine
{
    public class Sprite: IEquatable<Sprite>
    {
        public List<SpriteFrame> Frames { get; set; }
        public int NumFrames { get; set; }
        public int ActualFrame { get; set; }
        public uint LastUpdate { get; set; }
        public uint Fps { get; set; }
        public float RotationZ { get; set; }
        public string Name { get; set; }
        public bool LoopAnimation { get; set; }

        public Sprite()
        {
            Frames = new List<SpriteFrame>();
            Name = string.Empty;
        }

        public Sprite(Sprite sprite)
        {
            NumFrames = sprite.NumFrames;
            ActualFrame = sprite.ActualFrame;
            LastUpdate = sprite.LastUpdate;
            Fps = sprite.Fps;
            RotationZ = sprite.RotationZ;
            Name = new string(sprite.Name);
            LoopAnimation = sprite.LoopAnimation;
            Frames = sprite.Frames.Select(spriteFrame => new SpriteFrame(spriteFrame)).ToList();
        }

        public bool Equals(Sprite? other)
        {
            if (other is null)
            {
                return false;
            }

            return NumFrames.Equals(other.NumFrames) &&
                   ActualFrame.Equals(other.ActualFrame) &&
                   LastUpdate.Equals(other.LastUpdate) &&
                   Fps.Equals(other.Fps) &&
                   RotationZ.Equals(other.RotationZ) &&
                   Name.Equals(other.Name) &&
                   LoopAnimation.Equals(other.LoopAnimation) &&
                   Frames.SequenceEqual(other.Frames);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Sprite sprite)
            {
                return Equals(sprite);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = NumFrames.GetHashCode() ^
                   ActualFrame.GetHashCode() ^
                   LastUpdate.GetHashCode() ^
                   Fps.GetHashCode() ^
                   RotationZ.GetHashCode() ^
                   Name.GetHashCode() ^
                   LoopAnimation.GetHashCode();
            Frames.ForEach(frame => hashCode = hashCode ^frame.GetHashCode());
            return hashCode;
                    
        }

        public void Draw(SKPaintSurfaceEventArgs eventArgs, Vector2 position, Vector2 scale, float rotation, float width, float height)
        {
            if (Frames.Count <= 0)
            {
                return;
            }

            Frames[ActualFrame].Draw(eventArgs, position, scale, rotation, width, height);
            LastUpdate++;
            if (Fps < LastUpdate)
            {
                ActualFrame = (Frames.Count > ActualFrame) ? ActualFrame++ : LoopAnimation ? 0 : ActualFrame;
            }
        }

        public void LoadTextures(string sceneFolder)
        {
            foreach (SpriteFrame frame in Frames)
            {
                frame.LoadTexture(sceneFolder);
            }
        }
    }
}