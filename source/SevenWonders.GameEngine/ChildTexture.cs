using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWonders.GameEngine
{
    public class ChildTexture : ChildObject, IEquatable<ChildTexture>
    {
        public int TextureId { get; set; }

        public ChildTexture() : base()
        {
            TextureId = -1;
        }

        public ChildTexture(ChildTexture other): base(other)
        {
            TextureId = other.TextureId;
        }

        public bool Equals(ChildTexture? other)
        {
            if (other is null)
            {
                return false;
            }

            return base.Equals(other) &&
                   TextureId.Equals(other.TextureId);
        }

        public override bool Equals(ChildObject? other)
        {
            if (other is ChildTexture childTexture)
            {
                return Equals(childTexture);
            }

            return false;
        }

        public override bool Equals(object? obj)
        {
            if (obj is ChildTexture childTexture)
            {
                return Equals(childTexture);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetBaseHashCode() ^
                   TextureId.GetHashCode();
        }

        public override void Draw(SKPaintSurfaceEventArgs eventArgs, Vector2 parentPosition, Vector2 parentVisualSize, float parentRotation, float parentWidth, float parentHeight, TextureRegistry textureRegistry)
        {
            var childWidth = parentWidth * WidthPercent;
            var childHeight = parentHeight * HeightPercent;

            var scaledParentWidth = parentWidth * parentVisualSize.X;
            var scaledParentHeight = parentHeight * parentVisualSize.Y;
            var scaledChildWidth = childWidth * parentVisualSize.X;
            var scaledChildHeight = childHeight * parentVisualSize.Y;

            // Offset from parent center in unrotated space
            var offsetX = -scaledParentWidth / 2 + scaledChildWidth / 2 + PositionPercent.X * scaledParentWidth;
            var offsetY = -scaledParentHeight / 2 + scaledChildHeight / 2 + PositionPercent.Y * scaledParentHeight;

            // Rotate the offset by the parent's rotation angle
            var radians = parentRotation * MathF.PI / 180f;
            var cos = MathF.Cos(radians);
            var sin = MathF.Sin(radians);
            var rotatedOffsetX = offsetX * cos - offsetY * sin;
            var rotatedOffsetY = offsetX * sin + offsetY * cos;

            var childPosition = new Vector2(
                parentPosition.X + rotatedOffsetX,
                parentPosition.Y + rotatedOffsetY);

            textureRegistry.Get(TextureId).Draw(eventArgs, childPosition, parentVisualSize, parentRotation, childWidth, childHeight);
        }
    }
}
