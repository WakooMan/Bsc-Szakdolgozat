using System.Numerics;

namespace SevenWonders.GameEngine
{
    public class GameObject : IEquatable<GameObject>
    {
        public string Name { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public Vector2 Scale { get; set; }
        public List<Sprite> Animations { get; set; }
        public float Speed { get; set; }
        public float Rotation { get; set; }
        public bool Visible { get; set; }
        public bool Collidable { get; set; }
        public bool InFrustum { get; set; }
        public uint CurrentAnim { get; set; }
        public uint NumberOfFrames { get; set; }
        public uint ID { get; set; }
        public int Zindex { get; set; }
        public GraphicsLayer ParentLayer { get; set; }

        public GameObject()
        {
            Name = string.Empty;
            Animations = new List<Sprite>();
            ParentLayer = new GraphicsLayer();
        }

        public GameObject(GameObject gameObject)
        {
            Name = gameObject.Name;
            Position = gameObject.Position;
            Direction = gameObject.Direction;
            Scale = gameObject.Scale;
            Rotation = gameObject.Rotation;
            Visible = gameObject.Visible;
            Collidable = gameObject.Collidable;
            InFrustum = gameObject.InFrustum;
            Animations = gameObject.Animations.Select(sprite => new Sprite(sprite)).ToList();
            ParentLayer = new GraphicsLayer(gameObject.ParentLayer);
            CurrentAnim = gameObject.CurrentAnim;
            NumberOfFrames = gameObject.NumberOfFrames;
            ID = gameObject.ID;
            Zindex = gameObject.Zindex;
            Speed = gameObject.Speed;
        }

        public bool Equals(GameObject? other)
        {
            if (other is null)
            {
                return false;
            }

            return Name.Equals(other.Name) &&
                   Position.Equals(other.Position) &&
                   Direction.Equals(other.Direction) &&
                   Scale.Equals(other.Scale) &&
                   Rotation.Equals(other.Rotation) &&
                   Visible.Equals(other.Visible) &&
                   Collidable.Equals(other.Collidable) &&
                   InFrustum.Equals(other.InFrustum) &&
                   Animations.SequenceEqual(other.Animations) &&
                   ParentLayer.Equals(other.ParentLayer) && 
                   CurrentAnim.Equals(other.CurrentAnim) &&
                   NumberOfFrames.Equals(other.NumberOfFrames) &&
                   ID.Equals(other.ID) &&
                   Zindex.Equals(other.Zindex) &&
                   Speed.Equals(other.Speed);
        }

        public override bool Equals(object? obj)
        {
            if (obj is GameObject gameObject)
            {
                return Equals(gameObject);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() +
                   Position.GetHashCode() +
                   Direction.GetHashCode() +
                   Scale.GetHashCode() +
                   Rotation.GetHashCode() +
                   Visible.GetHashCode() +
                   Collidable.GetHashCode() +
                   InFrustum.GetHashCode() +
                   ParentLayer.GetHashCode() +
                   CurrentAnim.GetHashCode() +
                   NumberOfFrames.GetHashCode() +
                   ID.GetHashCode() +
                   Zindex.GetHashCode() +
                   Speed.GetHashCode() +
                   Animations.Select(anim => anim.GetHashCode()).Sum();
        }
    }
}
