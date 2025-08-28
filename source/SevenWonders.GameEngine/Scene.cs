using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWonders.GameEngine
{
    public class Scene: IEquatable<Scene>
    {
        public List<GraphicsLayer> Layers { get; set; }
        public string Name { get; set; }
        public bool Visible { get; set; }
        public int Id { get; set; }

        public Scene()
        {
            Layers = new List<GraphicsLayer>();
            Name = string.Empty;
        }

        public Scene(Scene scene)
        {
            Layers = scene.Layers.Select(layer => new GraphicsLayer(layer)).ToList();
            Name = scene.Name;
            Visible = scene.Visible;
            Id = scene.Id;
        }

        public bool Equals(Scene? other)
        {
            if (other is null)
            {
                return false;
            }

            return Layers.SequenceEqual(other.Layers) &&
                   Name.Equals(other.Name) &&
                   Id.Equals(other.Id) &&
                   Visible.Equals(other.Visible);  
        }

        public override bool Equals(object? obj)
        {
            if (obj is Scene scene)
            {
                return Equals(scene);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = Name.GetHashCode() ^
                   Visible.GetHashCode() ^
                   Id.GetHashCode();
            Layers.ForEach(layer => hashCode = hashCode ^ layer.GetHashCode());
            return hashCode;
        }

        public void Draw(SKPaintSurfaceEventArgs eventArgs)
        {
            if(!Visible)
                return;
            foreach (GraphicsLayer layer in Layers)
            {
                layer.Draw(eventArgs);
            }
        }

        public void LoadTextures(string sceneFolder)
        {
            foreach (GraphicsLayer layer in Layers)
            {
                foreach (Texture texture in layer.Textures)
                {
                    texture.LoadTexture(sceneFolder);
                }

                foreach (GameObject gameObject in layer.ObjectList)
                {
                    gameObject.LoadTextures(sceneFolder);
                }
            }
        }
    }
}