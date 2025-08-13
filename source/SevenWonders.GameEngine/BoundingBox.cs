using System.Numerics;

namespace SevenWonders.GameEngine
{
    public class BoundingBox: IEquatable<BoundingBox>
    {
        public Vector2 Minpoint { get; set; }
        public Vector2 Maxpoint { get; set; }
        public List<Vector2> BoxPoints { get; set; }
        public float BoxHalfWidth { get; set; }
        public float BoxHalfHeight { get; set; }
        public Matrix4x4 Matrix { get; set; }
        public bool Enabled { get; set; }

        public BoundingBox()
        {
            BoxPoints = new List<Vector2>();
        }

        public BoundingBox(BoundingBox boundingBox)
        {
            Minpoint = boundingBox.Minpoint;
            Maxpoint = boundingBox.Maxpoint;
            BoxPoints = [.. boundingBox.BoxPoints];
            BoxHalfHeight = boundingBox.BoxHalfHeight;
            BoxHalfWidth = boundingBox.BoxHalfWidth;
            Matrix = boundingBox.Matrix;
            Enabled = boundingBox.Enabled;
        }

        public bool Equals(BoundingBox? other)
        {
            if (other is null)
            {
                return false;
            }

            return Minpoint.Equals(other.Minpoint) &&
                   Maxpoint.Equals(other.Maxpoint) &&
                   BoxPoints.SequenceEqual(other.BoxPoints) &&
                   BoxHalfHeight.Equals(other.BoxHalfHeight) &&
                   BoxHalfWidth.Equals(other.BoxHalfWidth) &&
                   Matrix.Equals(other.Matrix) &&
                   Enabled.Equals(other.Enabled);
        }

        public override bool Equals(object? obj)
        {
            if (obj is BoundingBox boundingBox)
            {
                return Equals(boundingBox);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Minpoint.GetHashCode() +
                   Maxpoint.GetHashCode() +
                   BoxPoints.Select(boxPoint => boxPoint.GetHashCode()).Sum() +
                   BoxHalfHeight.GetHashCode() +
                   BoxHalfWidth.GetHashCode() +
                   Matrix.GetHashCode() +
                   Enabled.GetHashCode();
        }
    }
}