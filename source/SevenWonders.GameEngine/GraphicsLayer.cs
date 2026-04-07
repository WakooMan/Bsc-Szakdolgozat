using SkiaSharp.Views.Maui;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Xml.Serialization;

namespace SevenWonders.GameEngine
{
    public class GraphicsLayer : IEquatable<GraphicsLayer>
    {
        public bool Visible { get; set; }
        public bool EnableCollision { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public int ZIndex { get; set; }
        public List<SceneObject> SceneObjectsProxy { get; set; }

        [XmlIgnore]
        public IReadOnlyList<IInteractiveObject> InteractiveObjects => SceneObjectsProxy.OfType<IInteractiveObject>().ToList();

        [XmlIgnore]
        public IReadOnlyList<GameObject> GameObjects => SceneObjectsProxy.OfType<GameObject>().ToList();

        [XmlIgnore]
        public IReadOnlyList<ButtonObject> ButtonObjects => SceneObjectsProxy.OfType<ButtonObject>().ToList();

        [XmlIgnore]
        public IReadOnlyList<TextLabel> TextLabels => SceneObjectsProxy.OfType<TextLabel>().ToList();

        [XmlIgnore]
        public IReadOnlyList<TextureObject> TextureObjects => SceneObjectsProxy.OfType<TextureObject>().ToList();

        [XmlIgnore]
        public IReadOnlyList<SceneObject> SceneObjects => SceneObjectsProxy;


        public GraphicsLayer()
        {
            SceneObjectsProxy = new List<SceneObject>();
            Name = string.Empty;
        }

        public GraphicsLayer(GraphicsLayer graphicsLayer)
        {
            SceneObjectsProxy = graphicsLayer.SceneObjectsProxy.Select(obj => obj.Clone()).ToList();
            Visible = graphicsLayer.Visible;
            EnableCollision = graphicsLayer.EnableCollision;
            Name = new string(graphicsLayer.Name);
            Id = graphicsLayer.Id;
            ZIndex = graphicsLayer.ZIndex;
        }

        internal void AddSceneObject(SceneObject sceneObject)
        {
            var comparer = new SceneObjectComparer();
            int index = SceneObjectsProxy.BinarySearch(sceneObject, comparer);
            SceneObjectsProxy.Insert(index < 0 ? ~index : index, sceneObject);
            sceneObject.OnZIndexChanged += OnZIndexChanged;
        }

        internal void RemoveSceneObject(SceneObject sceneObject)
        {
            SceneObjectsProxy.Remove(sceneObject);
            sceneObject.OnZIndexChanged -= OnZIndexChanged;
        }

        public bool Equals(GraphicsLayer? other)
        {
            if (other is null)
            {
                return false;
            }

            return SceneObjectsProxy.SequenceEqual(other.SceneObjectsProxy) &&
                   Name.Equals(other.Name) &&
                   Id.Equals(other.Id) &&
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
            Id.GetHashCode() ^
            Visible.GetHashCode() ^
            EnableCollision.GetHashCode() ^
            ZIndex.GetHashCode();
            SceneObjectsProxy.ForEach(obj => hashCode = hashCode ^ obj.GetHashCode());
            return hashCode;
        }

        [ExcludeFromCodeCoverage]
        public void Draw(SKPaintSurfaceEventArgs eventArgs, TextureRegistry textureRegistry)
        {
            if (!Visible)
            {
                return;
            }

            foreach (var sceneObject in SceneObjectsProxy)
            {
                sceneObject.Draw(eventArgs, textureRegistry);
            }
        }

        internal void Resize(Vector2 oldResolution, Vector2 newResolution)
        {
            SceneObjectsProxy.ForEach(sceneObject => sceneObject.Resize(oldResolution, newResolution));
        }

        internal void SortAllObjects()
        {
            var comparer = new SceneObjectComparer();
            SceneObjectsProxy.Sort(comparer);
            SceneObjectsProxy.ForEach(sceneObject => sceneObject.OnZIndexChanged += OnZIndexChanged);
        }

        private void OnZIndexChanged(SceneObject sceneObject)
        {
            RemoveSceneObject(sceneObject);
            AddSceneObject(sceneObject);
        }
    }
}