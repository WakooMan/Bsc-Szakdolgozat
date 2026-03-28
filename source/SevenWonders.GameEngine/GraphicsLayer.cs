using SkiaSharp.Views.Maui;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace SevenWonders.GameEngine
{
    public class GraphicsLayer:IEquatable<GraphicsLayer>
    {
        public List<GameObject> ObjectList { get; set; }
        public List<TextureObject> TextureObjects { get; set; }
        public List<ButtonObject> Buttons { get; set; }
        public List<TextLabel> TextLabels { get; set; }
        public  bool Visible { get; set; }
        public bool EnableCollision { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public int ZIndex { get; set; }

        public GraphicsLayer()
        {
            ObjectList = new List<GameObject>();
            TextureObjects = new List<TextureObject>();
            Buttons = new List<ButtonObject>();
            TextLabels = new List<TextLabel>();
            Name = string.Empty;
        }

        public GraphicsLayer(GraphicsLayer graphicsLayer)
        {
            ObjectList = graphicsLayer.ObjectList.Select(obj => new GameObject(obj)).ToList();
            TextureObjects = graphicsLayer.TextureObjects.Select(texture => new TextureObject(texture)).ToList();
            Buttons = graphicsLayer.Buttons.Select(button => new ButtonObject(button)).ToList();
            TextLabels = graphicsLayer.TextLabels.Select(label => new TextLabel(label)).ToList();
            Visible = graphicsLayer.Visible;
            EnableCollision = graphicsLayer.EnableCollision;
            Name = new string(graphicsLayer.Name);
            Id = graphicsLayer.Id;
            ZIndex = graphicsLayer.ZIndex;
        }

        public bool Equals(GraphicsLayer? other)
        {
            if (other is null)
            {
                return false;
            }

            return ObjectList.SequenceEqual(other.ObjectList) &&
                   TextureObjects.SequenceEqual(other.TextureObjects) &&
                   Buttons.SequenceEqual(other.Buttons) &&
                   TextLabels.SequenceEqual(other.TextLabels) &&
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
            ObjectList.ForEach(obj => hashCode = hashCode ^ obj.GetHashCode());
            TextureObjects.ForEach(texture => hashCode = hashCode ^ texture.GetHashCode());
            Buttons.ForEach(button => hashCode = hashCode ^ button.GetHashCode());
            TextLabels.ForEach(label => hashCode = hashCode ^ label.GetHashCode());
            return hashCode;
        }

        [ExcludeFromCodeCoverage]
        public void Draw(SKPaintSurfaceEventArgs eventArgs, TextureRegistry textureRegistry)
        {
            if (!Visible)
            {
                return;
            }

            List<TextureObject> textures = [.. TextureObjects];
            textures.Sort(new TextureObjectComparer());
            foreach (var texture in textures)
            {
                texture.Draw(eventArgs, textureRegistry);
            }

            List<GameObject> gameObjects = [.. ObjectList];
            gameObjects.Sort(new GameObjectComparer());

            foreach (var gameObject in gameObjects)
            {
                gameObject.Draw(eventArgs, textureRegistry);
            }

            List<ButtonObject> buttons = [.. Buttons];
            buttons.Sort((a, b) => a.ZIndex.CompareTo(b.ZIndex));
            foreach (var button in buttons)
            {
                button.Draw(eventArgs, textureRegistry);
            }

            List<TextLabel> textLabels = [.. TextLabels];
            textLabels.Sort((a, b) => a.ZIndex.CompareTo(b.ZIndex));
            foreach (var label in textLabels)
            {
                label.Draw(eventArgs, textureRegistry);
            }
        }

        public void Resize(Vector2 oldResolution, Vector2 newResolution)
        {
            TextureObjects.ForEach(texture => texture.Resize(oldResolution, newResolution));
            ObjectList.ForEach(gameObject => gameObject.Resize(oldResolution, newResolution));
            Buttons.ForEach(button => button.Resize(oldResolution, newResolution));
            TextLabels.ForEach(label => label.Resize(oldResolution, newResolution));
        }
    }
}