using System.Numerics;

namespace SevenWonders.GameEngine
{
    /// <summary>
    /// Base class for all drawable, positionable objects in a scene.
    /// Provides the common properties shared by <see cref="TextureObject"/>,
    /// <see cref="TextLabel"/> and <see cref="GameObject"/>.
    /// </summary>
    public abstract class SceneObject : IEquatable<SceneObject>
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int ZIndex { get; set; }
        public bool Visible { get; set; }
        public Vector2 Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; }

        protected SceneObject()
        {
            Name = string.Empty;
            Scale = new Vector2(1, 1);
        }

        protected SceneObject(SceneObject other)
        {
            Name = new string(other.Name);
            Id = other.Id;
            ZIndex = other.ZIndex;
            Visible = other.Visible;
            Position = other.Position;
            Width = other.Width;
            Height = other.Height;
            Rotation = other.Rotation;
            Scale = other.Scale;
        }

        public virtual void Resize(Vector2 oldResolution, Vector2 newResolution)
        {
            float xRatio = newResolution.X / oldResolution.X;
            float yRatio = newResolution.Y / oldResolution.Y;
            Position = new Vector2(Position.X * xRatio, Position.Y * yRatio);
            Width = Width * xRatio;
            Height = Height * yRatio;
        }

        public bool Equals(SceneObject? other)
        {
            if (other is null)
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        public override bool Equals(object? obj)
        {
            if (obj is SceneObject sceneObject)
            {
                return Equals(sceneObject);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
