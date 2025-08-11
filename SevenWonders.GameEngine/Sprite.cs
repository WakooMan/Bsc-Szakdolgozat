using System.Numerics;

namespace SevenWonders.GameEngine
{
    public class Sprite: IEquatable<Sprite>
    {
        public List<SpriteFrame> Frames { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
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
            Position = sprite.Position;
            Scale = sprite.Scale;
            NumFrames = sprite.NumFrames;
            ActualFrame = sprite.ActualFrame;
            LastUpdate = sprite.LastUpdate;
            Fps = sprite.Fps;
            RotationZ = sprite.RotationZ;
            Name = sprite.Name;
            LoopAnimation = sprite.LoopAnimation;
            Frames = sprite.Frames.Select(spriteFrame => new SpriteFrame(spriteFrame)).ToList();
        }

        public bool Equals(Sprite? other)
        {
            if (other is null)
            {
                return false;
            }

            return Position.Equals(other.Position) &&
                   Scale.Equals(other.Scale) &&
                   NumFrames.Equals(other.NumFrames) &&
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
            return Position.GetHashCode() +
                   Scale.GetHashCode() +
                   NumFrames.GetHashCode() +
                   ActualFrame.GetHashCode() +
                   LastUpdate.GetHashCode() +
                   Fps.GetHashCode() +
                   RotationZ.GetHashCode() +
                   Name.GetHashCode() +
                   LoopAnimation.GetHashCode() +
                   Frames.Select(frame => frame.GetHashCode()).Sum();
                    
        }
    }
}