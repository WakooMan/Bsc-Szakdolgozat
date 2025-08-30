using SkiaSharp.Views.Maui;
using System;
using System.Numerics;

namespace SevenWonders.GameEngine
{
    public class GraphicsLayer:IEquatable<GraphicsLayer>
    {
        public List<GameObject> ObjectList { get; set; }
        public List<Texture> Textures { get; set; }
        public  bool Visible { get; set; }
        public bool EnableCollision { get; set; }
        public string Name { get; set; }
        public int ID { get; set; }

        public GraphicsLayer()
        {
            ObjectList = new List<GameObject>();
            Textures = new List<Texture>();
            Name = string.Empty;
        }

        public GraphicsLayer(GraphicsLayer graphicsLayer)
        {
            ObjectList = graphicsLayer.ObjectList.Select(obj => new GameObject(obj)).ToList();
            Textures = graphicsLayer.Textures.Select(texture => new Texture(texture)).ToList();
            Visible = graphicsLayer.Visible;
            EnableCollision = graphicsLayer.EnableCollision;
            Name = graphicsLayer.Name;
            ID = graphicsLayer.ID;
        }

        public bool Equals(GraphicsLayer? other)
        {
            if (other is null)
            {
                return false;
            }

            return ObjectList.SequenceEqual(other.ObjectList) &&
                   Textures.SequenceEqual(other.Textures) &&
                   Name.Equals(other.Name) &&
                   ID.Equals(other.ID) &&
                   Visible.Equals(other.Visible) &&
                   EnableCollision.Equals(other.EnableCollision);
        }

        public override bool Equals(object? obj)
        {
            if (obj is GraphicsLayer graphicsLayer)
            {
                return Equals(graphicsLayer);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = Name.GetHashCode() ^
            ID.GetHashCode() ^
            Visible.GetHashCode() ^
            EnableCollision.GetHashCode();
            ObjectList.ForEach(obj => hashCode = hashCode ^ obj.GetHashCode());
            Textures.ForEach(texture => hashCode = hashCode ^ texture.GetHashCode());
            return hashCode;
        }

        public void Draw(SKPaintSurfaceEventArgs eventArgs)
        {
            foreach (var texture in Textures)
            {
                texture.Draw(eventArgs);
            }

            foreach (var gameObject in ObjectList)
            {
                gameObject.Draw(eventArgs);
            }
        }

        public void LoadTextures(string tempPath)
        {
            foreach (var texture in Textures)
            {
                texture.LoadTexture(tempPath);
            }

            foreach (var gameObject in ObjectList)
            {
                gameObject.LoadTextures(tempPath);
            }
        }
    }
}