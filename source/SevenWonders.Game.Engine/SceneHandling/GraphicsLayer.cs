using SevenWonders.Game.Engine.InputHandling;
using SevenWonders.Game.Engine.SceneObjects;
using SkiaSharp;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Xml.Serialization;

namespace SevenWonders.Game.Engine.SceneHandling
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
        public IReadOnlyList<IInteractiveObject> InteractiveObjects
        {
            get
            {
                lock (SceneObjectsProxy)
                {
                    return SceneObjectsProxy.OfType<IInteractiveObject>().ToList();
                }
            }
        }

        [XmlIgnore]
        public IReadOnlyList<GameObject> GameObjects
        {
            get
            {
                lock (SceneObjectsProxy)
                {
                    return SceneObjectsProxy.OfType<GameObject>().ToList();
                }
            }
        }


        [XmlIgnore]
        public IReadOnlyList<ButtonObject> ButtonObjects
        {
            get
            {
                lock (SceneObjectsProxy)
                {
                    return SceneObjectsProxy.OfType<ButtonObject>().ToList();
                }
            }
        }

        [XmlIgnore]
        public IReadOnlyList<TextLabel> TextLabels
        {
            get
            {
                lock (SceneObjectsProxy)
                {
                    return SceneObjectsProxy.OfType<TextLabel>().ToList();
                }
            }
        }

        [XmlIgnore]
        public IReadOnlyList<TextureObject> TextureObjects
        {
            get
            {
                lock (SceneObjectsProxy)
                {
                    return SceneObjectsProxy.OfType<TextureObject>().ToList();
                }
            }
        }

        [XmlIgnore]
        public IReadOnlyList<SceneObject> SceneObjects
        {
            get
            {
                lock (SceneObjectsProxy)
                {
                    return SceneObjectsProxy.ToList();
                }
            }
        }


        public GraphicsLayer()
        {
            SceneObjectsProxy = new List<SceneObject>();
            Name = string.Empty;
            m_staticPicture = null;
        }

        public GraphicsLayer(GraphicsLayer graphicsLayer)
        {
            lock (graphicsLayer.SceneObjectsProxy)
            {
                SceneObjectsProxy = graphicsLayer.SceneObjectsProxy.Select(obj => obj.Clone()).ToList();
            }
            Visible = graphicsLayer.Visible;
            EnableCollision = graphicsLayer.EnableCollision;
            Name = new string(graphicsLayer.Name);
            Id = graphicsLayer.Id;
            ZIndex = graphicsLayer.ZIndex;
            m_staticPicture = null;
        }

        internal void AddSceneObject(SceneObject sceneObject)
        {
            lock (SceneObjectsProxy)
            {
                var comparer = new SceneObjectComparer();
                int index = SceneObjectsProxy.BinarySearch(sceneObject, comparer);
                SceneObjectsProxy.Insert(index < 0 ? ~index : index, sceneObject);
                sceneObject.OnZIndexChanged += OnZIndexChanged;
            }
        }

        internal void RemoveSceneObject(SceneObject sceneObject)
        {
            lock (SceneObjectsProxy)
            {
                SceneObjectsProxy.Remove(sceneObject);
                sceneObject.OnZIndexChanged -= OnZIndexChanged;
            }
        }

        public bool Equals(GraphicsLayer? other)
        {
            if (other is null)
            {
                return false;
            }

            lock (SceneObjectsProxy)
            {
                return SceneObjectsProxy.SequenceEqual(other.SceneObjectsProxy) &&
                       Name.Equals(other.Name) &&
                       Id.Equals(other.Id) &&
                       Visible.Equals(other.Visible) &&
                       EnableCollision.Equals(other.EnableCollision) &&
                       ZIndex.Equals(other.ZIndex);
            }
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
            lock (SceneObjectsProxy)
            {
                SceneObjectsProxy.ForEach(obj => hashCode = hashCode ^ obj.GetHashCode());
            }
            return hashCode;
        }

        [ExcludeFromCodeCoverage]
        public void DrawStatic(SKCanvas canvas, TextureRegistry textureRegistry, float resolutionX, float resolutionY)
        {
            if (!Visible)
            {
                return;
            }

            lock (SceneObjectsProxy)
            {
                if (m_staticPicture is null)
                {
                    SKCanvas temporaryCanvas = m_recorder.BeginRecording(new SKRect(0, 0, resolutionX, resolutionY));
                    foreach (var sceneObject in SceneObjectsProxy.Where(obj => obj.IsStatic()))
                    {
                        sceneObject.Draw(temporaryCanvas, textureRegistry);
                    }
                    temporaryCanvas.ResetMatrix();
                    m_staticPicture = m_recorder.EndRecording();
                }
            }

            canvas.Save();
            canvas.DrawPicture(m_staticPicture);
            canvas.Restore();
        }

        [ExcludeFromCodeCoverage]
        public void Draw(SKCanvas canvas, TextureRegistry textureRegistry)
        {
            if (!Visible)
            {
                return;
            }

            lock (SceneObjectsProxy)
            {
                foreach (var sceneObject in SceneObjectsProxy.Where(obj => !obj.IsStatic()))
                {
                    canvas.Save();
                    sceneObject.Draw(canvas, textureRegistry);
                    canvas.Restore();
                }
            }
        }

        internal void Resize(Vector2 oldResolution, Vector2 newResolution)
        {
            lock (SceneObjectsProxy)
            {
                m_staticPicture?.Dispose();
                m_staticPicture = null;
                SceneObjectsProxy.ForEach(sceneObject => sceneObject.Resize(oldResolution, newResolution));
            }
        }

        internal void SortAllObjects()
        {
            lock (SceneObjectsProxy)
            {
                var comparer = new SceneObjectComparer();
                SceneObjectsProxy.Sort(comparer);
                SceneObjectsProxy.ForEach(sceneObject => sceneObject.OnZIndexChanged += OnZIndexChanged);
            }
        }

        private void OnZIndexChanged(SceneObject sceneObject)
        {
            lock (SceneObjectsProxy)
            {
                RemoveSceneObject(sceneObject);
                AddSceneObject(sceneObject);
            }
        }

        private SKPicture? m_staticPicture;
        private readonly SKPictureRecorder m_recorder = new SKPictureRecorder();

    }
}