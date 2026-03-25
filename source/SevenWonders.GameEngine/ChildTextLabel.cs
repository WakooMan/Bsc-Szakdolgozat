using SkiaSharp.Views.Maui;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace SevenWonders.GameEngine
{
    /// <summary>
    /// Wraps a <see cref="TextLabel"/> so it can be drawn as a child of a <see cref="SpriteFrame"/>,
    /// positioned and sized relative to the parent frame's bounds.
    /// </summary>
    public class ChildTextLabel : ChildObject, IEquatable<ChildTextLabel>
    {

        /// <summary>The <see cref="TextLabel"/> that will be rendered as a child.</summary>
        public TextLabel TextLabel { get; set; }

        public ChildTextLabel(): base()
        {
            TextLabel = new TextLabel();
        }

        public ChildTextLabel(ChildTextLabel other) : base(other)
        {
            TextLabel = new TextLabel(other.TextLabel);
        }

        public bool Equals(ChildTextLabel? other)
        {
            if (other is null)
            {
                return false;
            }

            return base.Equals(other) &&
                   TextLabel.Equals(other.TextLabel);
        }

        public override bool Equals(ChildObject? other)
        {
            if (other is ChildTextLabel childTextLabel)
            {
                return Equals(childTextLabel);
            }

            return false;
        }

        public override bool Equals(object? obj)
        {
            if (obj is ChildTextLabel childTextLabel)
            {
                return Equals(childTextLabel);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetBaseHashCode() ^
                   TextLabel.GetHashCode();
        }

        [ExcludeFromCodeCoverage]
        public override void Draw(SKPaintSurfaceEventArgs eventArgs, Vector2 parentPosition, Vector2 parentVisualSize,
                         float parentRotation, float parentWidth, float parentHeight, TextureRegistry textureRegistry)
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

            // Temporarily override the TextLabel's transform with the resolved child transform
            var originalPosition = TextLabel.Position;
            var originalWidth = TextLabel.Width;
            var originalHeight = TextLabel.Height;
            var originalRotation = TextLabel.Rotation;
            var originalScale = TextLabel.Scale;

            TextLabel.Position = childPosition;
            TextLabel.Width = childWidth;
            TextLabel.Height = childHeight;
            TextLabel.Rotation = parentRotation;
            TextLabel.Scale = parentVisualSize;

            TextLabel.Draw(eventArgs, textureRegistry);

            TextLabel.Position = originalPosition;
            TextLabel.Width = originalWidth;
            TextLabel.Height = originalHeight;
            TextLabel.Rotation = originalRotation;
            TextLabel.Scale = originalScale;
        }
    }
}
