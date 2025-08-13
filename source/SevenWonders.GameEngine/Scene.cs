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
            return Layers.Select(layer => layer.GetHashCode()).Sum() +
                   Name.GetHashCode() +
                   Visible.GetHashCode() +
                   Id.GetHashCode();
        }
    }
}