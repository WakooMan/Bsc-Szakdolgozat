using SevenWonders.Game.Engine.SceneHandling;
using SevenWonders.Game.Engine.SceneObjects;
using SkiaSharp;
using System.Numerics;

namespace SevenWonders.Game.Engine.ChildObjects
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
            return GetBaseHashCode() ^
                   TextureId.GetHashCode();
        }

        public override void Draw(SKCanvas canvas, Vector2 parentPosition, Vector2 parentVisualSize, float parentRotation, float parentWidth, float parentHeight, bool dimmed, TextureRegistry textureRegistry)
        {
            var childWidth = parentWidth * WidthPercent;
            var childHeight = parentHeight * HeightPercent;

            var scaledParentWidth = parentWidth * parentVisualSize.X;
            var scaledParentHeight = parentHeight * parentVisualSize.Y;
            var scaledChildWidth = childWidth * parentVisualSize.X;
            var scaledChildHeight = childHeight * parentVisualSize.Y;

            var offsetX = -scaledParentWidth / 2 + scaledChildWidth / 2 + PositionPercent.X * scaledParentWidth;
            var offsetY = -scaledParentHeight / 2 + scaledChildHeight / 2 + PositionPercent.Y * scaledParentHeight;

            var radians = parentRotation * MathF.PI / 180f;
            var cos = MathF.Cos(radians);
            var sin = MathF.Sin(radians);
            var rotatedOffsetX = offsetX * cos - offsetY * sin;
            var rotatedOffsetY = offsetX * sin + offsetY * cos;

            var childPosition = new Vector2(
                parentPosition.X + rotatedOffsetX,
                parentPosition.Y + rotatedOffsetY);

            Texture texture = textureRegistry.Get(TextureId);
            if (dimmed)
            {
                texture.CustomColorFilter = SKColorFilter.CreateBlendMode(
                    SKColors.Black.WithAlpha(120),
                    SKBlendMode.SrcOver
                );
            }
            else if (texture.CustomColorFilter is not null)
            {
                texture.CustomColorFilter = null;
            }
            texture.Draw(canvas, childPosition, parentVisualSize, parentRotation, childWidth, childHeight);
        }
    }
}
