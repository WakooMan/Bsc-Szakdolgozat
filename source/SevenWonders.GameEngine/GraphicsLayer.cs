using SkiaSharp.Views.Maui;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace SevenWonders.GameEngine
{
    public class GraphicsLayer:IEquatable<GraphicsLayer>
    {
        public List<GameObject> ObjectList { get; set; }
        public List<TextureObject> Textures { get; set; }
        public  bool Visible { get; set; }
        public bool EnableCollision { get; set; }
        public string Name { get; set; }
        public int ID { get; set; }
        public int ZIndex { get; set; }

        public GraphicsLayer()
        {
            ObjectList = new List<GameObject>();
            Textures = new List<TextureObject>();
            Name = string.Empty;
        }

        public GraphicsLayer(GraphicsLayer graphicsLayer)
        {
            ObjectList = graphicsLayer.ObjectList.Select(obj => new GameObject(obj)).ToList();
            Textures = graphicsLayer.Textures.Select(texture => new TextureObject(texture)).ToList();
            Visible = graphicsLayer.Visible;
            EnableCollision = graphicsLayer.EnableCollision;
            Name = new string(graphicsLayer.Name);
            ID = graphicsLayer.ID;
            ZIndex = graphicsLayer.ZIndex;
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
                   EnableCollision.Equals(other.EnableCollision) &&
                   ZIndex.Equals(other.ZIndex);
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
            EnableCollision.GetHashCode() ^
            ZIndex.GetHashCode();
            ObjectList.ForEach(obj => hashCode = hashCode ^ obj.GetHashCode());
            Textures.ForEach(texture => hashCode = hashCode ^ texture.GetHashCode());
            return hashCode;
        }

        [ExcludeFromCodeCoverage]
        public void Draw(SKPaintSurfaceEventArgs eventArgs)
        {
            if (!Visible)
            {
                return;
            }

            List<TextureObject> textures = [.. Textures];
            textures.Sort(new TextureObjectComparer());
            foreach (var texture in textures)
            {
                texture.Draw(eventArgs);
            }

            List<GameObject> gameObjects = [.. ObjectList];
            gameObjects.Sort(new GameObjectComparer());

            foreach (var gameObject in gameObjects)
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

        public void Resize(Vector2 oldResolution, Vector2 newResolution)
        {
            Textures.ForEach(texture => texture.Resize(oldResolution, newResolution));
            ObjectList.ForEach(gameObject => gameObject.Resize(oldResolution, newResolution));
        }
    }
}