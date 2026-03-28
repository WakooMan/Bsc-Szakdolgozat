using SkiaSharp.Views.Maui;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Xml.Serialization;

namespace SevenWonders.GameEngine
{
    public class Scene : IEquatable<Scene>
    {
        public Guid Id { get; set; }
        public HashSet<int> UsedIds => Layers
                .SelectMany(layer => layer.ObjectList.Select(o => o.Id)
                .Concat(layer.TextureObjects.Select(t => t.Id))
                .Concat(layer.Buttons.Select(b => b.Id))
                .Concat(layer.TextLabels.Select(tl => tl.Id))
                .Concat([layer.Id]))
                .Concat(Textures.Select(t => t.Id))
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
            Id = Guid.NewGuid();
            Layers = scene.Layers.Select(layer => new GraphicsLayer(layer)).ToList();
            Textures = scene.Textures.Select(texture => new Texture(texture)).ToList();
            Name = scene.Name;
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
        public void Draw(SKPaintSurfaceEventArgs eventArgs)
        {
            if (!Visible)
                return;

            List<GraphicsLayer> objects = [.. Layers];
            objects.Sort(new GraphicsLayerComparer());

            foreach (GraphicsLayer layer in objects)
            {
                layer.Draw(eventArgs, TextureRegistry);
            }
        }

        public void LoadTextures(string sceneFolder)
        {
            foreach (Texture texture in Textures)
            {
                texture.LoadTexture(sceneFolder);
            }

            InitializeTextureRegistry();
        }

        public void AddTexture(Texture texture, string sceneFolder)
        {
            texture.LoadTexture(sceneFolder);
            Textures.Add(texture);
            TextureRegistry.Register(texture);
        }

        public void Resize(Vector2 newResolution)
        {
            Layers.ForEach(layer => layer.Resize(Resolution, newResolution));
            Resolution = newResolution;
        }

        private void InitializeTextureRegistry()
        {
            TextureRegistry.Clear();
            TextureRegistry.Register(Textures);
        }
    }
}