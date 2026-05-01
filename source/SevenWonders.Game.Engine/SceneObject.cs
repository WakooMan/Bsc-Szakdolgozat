using SkiaSharp;
using System.Numerics;
using System.Xml.Serialization;

namespace SevenWonders.Game.Engine
{
    /// <summary>
    /// Base class for all drawable, positionable objects in a scene.
    /// Provides the common properties shared by <see cref="TextureObject"/>,
    /// <see cref="TextLabel"/> and <see cref="GameObject"/>.
    /// </summary>
    [XmlInclude(typeof(GameObject))]
    [XmlInclude(typeof(TextLabel))]
    [XmlInclude(typeof(TextureObject))]
    public abstract class SceneObject : IEquatable<SceneObject>
    {
        public event Action<SceneObject>? OnZIndexChanged;

        public string Name { get; set; }
        public int Id { get; set; }
        public int ZIndex
        { 
            get
            {
                return m_zIndex;
            }
            set
            {
                if (m_zIndex != value)
                {
                    m_zIndex = value;
                    OnZIndexChanged?.Invoke(this);
                }
            }
        }
        public bool Visible { get; set; }
        public Vector2 Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; }
        [XmlIgnore]
        public bool Dimmed { get; set; }

        protected SceneObject()
        {
            Name = string.Empty;
            Scale = new Vector2(1, 1);
            OnZIndexChanged = null;
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
            Dimmed = other.Dimmed;
            OnZIndexChanged = null;
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

        public abstract void Draw(SKCanvas canvas, TextureRegistry textureRegistry);

        public abstract SceneObject Clone();

        private int m_zIndex;
    }
}
