using SkiaSharp.Views.Maui;
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
        public int CurrentAnim { get; set; }
        public int NumberOfFrames { get; set; }
        public int Id { get; set; }
        public int Zindex { get; set; }

        public GameObject()
        {
            Name = string.Empty;
            Animations = new List<Sprite>();
            CurrentAnim = 0;
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
            CurrentAnim = gameObject.CurrentAnim;
            NumberOfFrames = gameObject.NumberOfFrames;
            Id = gameObject.Id;
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
                   CurrentAnim.Equals(other.CurrentAnim) &&
                   NumberOfFrames.Equals(other.NumberOfFrames) &&
                   Id.Equals(other.Id) &&
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
            int hashCode = Name.GetHashCode() ^
                   Position.GetHashCode() ^
                   Direction.GetHashCode() ^
                   Scale.GetHashCode() ^
                   Rotation.GetHashCode() ^
                   Visible.GetHashCode() ^
                   Collidable.GetHashCode() ^
                   InFrustum.GetHashCode() ^
                   CurrentAnim.GetHashCode() ^
                   NumberOfFrames.GetHashCode() ^
                   Id.GetHashCode() ^
                   Zindex.GetHashCode() ^
                   Speed.GetHashCode();
            Animations.ForEach(anim => hashCode = hashCode ^ anim.GetHashCode());
            return hashCode;
        }

        public void LoadTextures(string sceneFolder)
        {
            foreach (Sprite sprite in Animations)
            {
                sprite.LoadTextures(sceneFolder);
            }
        }

        public void Draw(SKPaintSurfaceEventArgs eventArgs)
        {
            if (!Visible || Animations.Count <= 0)
                return;

            Animations[CurrentAnim].Draw(eventArgs, Position, Scale, Rotation);
        }
    }
}
