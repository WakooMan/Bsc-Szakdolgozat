using SevenWonders.Game.Engine.SceneHandling;
using SevenWonders.Game.Engine.SceneObjects;
using SkiaSharp;
using System.Numerics;

namespace SevenWonders.Game.Engine.ChildObjects
{
    /// <summary>
    /// Represents an object that can be drawn as a child of a <see cref="SpriteFrame"/>,
    /// positioned and sized relative to the parent frame's bounds.
    /// </summary>
    public abstract class ChildObject : IEquatable<ChildObject>
    {
        public string Name { get; set; }

        /// <summary>Width of the child as a fraction of the parent frame's width (0.0 – 1.0).</summary>
        public float WidthPercent { get; set; }

        /// <summary>Height of the child as a fraction of the parent frame's height (0.0 – 1.0).</summary>
        public float HeightPercent { get; set; }

        /// <summary>
        /// Position offset as a fraction of the parent frame's width/height,
        /// where (0, 0) is the top-left corner of the parent.
        /// </summary>
        public Vector2 PositionPercent { get; set; }

        protected ChildObject()
        {
        }

        protected ChildObject(ChildObject other)
        {
            WidthPercent = other.WidthPercent;
            HeightPercent = other.HeightPercent;
            PositionPercent = other.PositionPercent;
        }

        /// <summary>
        /// Draws the child object relative to the parent frame's position and transform.
        /// </summary>
        public abstract void Draw(SKCanvas canvas, Vector2 parentPosition, Vector2 parentVisualSize,
                  float parentRotation, float parentWidth, float parentHeight, bool dimmed, TextureRegistry textureRegistry);

        public virtual bool Equals(ChildObject? other)
        {
            if (other is not null)
            { 
                return WidthPercent.Equals(other.WidthPercent) &&
                       HeightPercent.Equals(other.HeightPercent) &&
                       PositionPercent.Equals(other.PositionPercent);
            }
            return false;
        }

        public abstract override int GetHashCode();

        protected int GetBaseHashCode()
        {
            return WidthPercent.GetHashCode() ^
                   HeightPercent.GetHashCode() ^
                   PositionPercent.GetHashCode();
        }
    }
}
