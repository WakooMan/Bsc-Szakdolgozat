using SevenWonders.Game.Engine.SceneObjects;
using SkiaSharp;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Xml.Serialization;

namespace SevenWonders.Game.Engine.SceneHandling
{
    public class Scene : IEquatable<Scene>, IDisposable
    {
        public Guid Id { get; set; }
        [XmlIgnore]
        public HashSet<int> UsedIds => Layers
                .Select(layer => layer.Id)
                .Concat(Textures.Select(texture => texture.Id))
                .Concat(Layers.SelectMany(layer => layer.SceneObjects.Select(sceneObject => sceneObject.Id)))
                .ToHashSet();
        public List<GraphicsLayer> Layers { get; set; }
        public List<Texture> Textures { get; set; }
        public string Name { get; set; }
        public bool Visible { get; set; }
        public Vector2 Resolution { get; set; }

        [XmlIgnore]
        public TextureRegistry TextureRegistry { get; } = new();

        public Scene()
        {
            Id = Guid.Empty;
            Layers = new List<GraphicsLayer>();
            Textures = new List<Texture>();
            Resolution = new Vector2(3840, 2160);
            Name = string.Empty;
        }

        public Scene(Scene scene)
        {
            Name = scene.Name;
            Id = Guid.NewGuid();
            Textures = scene.Textures.Select(texture => new Texture(texture)).ToList();
            Layers = scene.Layers.Select(layer => new GraphicsLayer(layer)).ToList();
            Visible = scene.Visible;
        }

        public void CopyFrom(Scene scene)
        {
            Layers = scene.Layers.Select(layer => new GraphicsLayer(layer)).ToList();
            Visible = scene.Visible;
        }

        public bool Equals(Scene? other)
        {
            if (other is null)
            {
                return false;
            }

            return Layers.SequenceEqual(other.Layers) &&
                   Textures.SequenceEqual(other.Textures) &&
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
            Textures.ForEach(texture => hashCode = hashCode ^ texture.GetHashCode());
            return hashCode;
        }

        [ExcludeFromCodeCoverage]
        public void Draw(SKCanvas canvas)
        {
            if (!Visible)
                return;

            foreach (GraphicsLayer layer in Layers)
            {
                layer.Draw(canvas, TextureRegistry);
            }
        }

        public void LoadTextures(string sceneFolder)
        {
            foreach (Texture texture in Textures)
            {
                texture.LoadTexture(sceneFolder);
            }

            SortAllLayers();
            InitializeTextureRegistry();
        }

        public void AddLayer(GraphicsLayer layer)
        {
            var comparer = new GraphicsLayerComparer();
            int index = Layers.BinarySearch(layer, comparer);
            Layers.Insert(index < 0 ? ~index : index, layer);
        }

        public void AddTexture(Texture texture, string sceneFolder)
        {
            texture.LoadTexture(sceneFolder);
            Textures.Add(texture);
            TextureRegistry.Register(texture);
        }

        public void Resize(Vector2 newResolution)
        {
            Textures.ForEach(texture => texture.ClearCache());
            Layers.ForEach(layer => layer.Resize(Resolution, newResolution));
            Resolution = newResolution;
        }

        public void InitializeTextureRegistry()
        {
            TextureRegistry.Clear();
            TextureRegistry.Register(Textures);
        }

        private void SortAllLayers()
        {
            Layers.Sort(new GraphicsLayerComparer());
            foreach (GraphicsLayer layer in Layers)
            {
                layer.SortAllObjects();
            }
        }

        public void Dispose()
        {
            Textures.ForEach(texture => texture.Dispose());
        }
    }
}