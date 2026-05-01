using SevenWonders.Common;
using SkiaSharp;
using System.Diagnostics.CodeAnalysis;
using static SevenWonders.Game.Engine.ISceneManager;

namespace SevenWonders.Game.Engine
{
    public class SceneManager: ISceneManager
    {
        public event SceneEvent SceneRegistered = delegate { };
        public event SceneEvent SceneRemoved = delegate { };

        public bool DrawBoundingBox;
        public SKColor BoundingShapeColor;
        public Scene? CurrentScene { get; private set; }
        public IReadOnlyList<Scene> Scenes => m_scenes;

        public SceneManager()
        {
            m_scenes = new List<Scene>();
        }

        public void SetCurrentScene(Scene scene)
        {
            ArgumentChecker.CheckPredicateForArgument(() => !m_scenes.Contains(scene), $"The scene \"{scene.Name}\" is not registered!");
            CurrentScene = scene;
        }

        public void RegisterScene(Scene scene)
        {
            if (!m_scenes.Contains(scene) && m_scenes.All(sc => sc.Name != scene.Name && sc.Id != scene.Id))
            {
                m_scenes.Add(scene);
                SceneRegistered(scene);
            }
        }

        [ExcludeFromCodeCoverage]
        public void Render(SKCanvas canvas)
        {
            if (CurrentScene is null)
            {
                return;
            }
            canvas.Clear(SKColors.Black);
            CurrentScene.Draw(canvas);
        }
        public GameObject? GetObjectByName(string name)
        {
            if (CurrentScene is null)
            {
                return null;
            }

            foreach (GraphicsLayer graphicsLayer in CurrentScene.Layers)
            {
                foreach (GameObject gameObject in graphicsLayer.GameObjects)
                {
                    if (gameObject.Name.ToLower() == name.ToLower())
                    {
                        return gameObject;
                    }
                }
            }

            return null;
        }

        public TextLabel? GetTextLabelByName(string name)
        {
            if (CurrentScene is null)
            {
                return null;
            }

            foreach (GraphicsLayer graphicsLayer in CurrentScene.Layers)
            {
                foreach (TextLabel textLabel in graphicsLayer.TextLabels)
                {
                    if (textLabel.Name.ToLower() == name.ToLower())
                    {
                        return textLabel;
                    }
                }
            }

            return null;
        }

        public IInteractiveObject? GetInteractiveObjectByName(string name)
        {
            if (CurrentScene is null)
            {
                return null;
            }

            foreach (GraphicsLayer graphicsLayer in CurrentScene.Layers)
            {
                foreach (IInteractiveObject interactiveObject in graphicsLayer.InteractiveObjects)
                {
                    if (interactiveObject.Name.ToLower() == name.ToLower())
                    {
                        return interactiveObject;
                    }
                }
            }

            return null;
        }

        public ButtonObject? GetButtonByName(string name)
        {
            if (CurrentScene is null)
            {
                return null;
            }

            foreach (GraphicsLayer graphicsLayer in CurrentScene.Layers)
            {
                foreach (ButtonObject button in graphicsLayer.ButtonObjects)
                {
                    if (button.Name.ToLower() == name.ToLower())
                    {
                        return button;
                    }
                }
            }

            return null;
        }

        public GraphicsLayer? GetLayerByName(string name)
        {
            if (CurrentScene is null)
            {
                return null;
            }

            foreach (GraphicsLayer graphicsLayer in CurrentScene.Layers)
            {
                if (graphicsLayer.Name.ToLower() == name.ToLower())
                {
                    return graphicsLayer;
                }
            }

            return null;
        }

        public Scene GetScene(Guid sceneID)
        {
            return m_scenes.First(scene => scene.Id == sceneID);
        }
        public Scene GetSceneByName(string name)
        {
            return m_scenes.First(scene => scene.Name == name);
        }
        public void FreeObject(int id)
        {

        }
        public void FreeObjects()
        {

        }
        public void Clear()
        {
            m_scenes.ForEach(scene => SceneRemoved(scene));
            m_scenes.Clear();
        }
        public void FreeAScene(string name)
        {
            Scene? scene = m_scenes.FirstOrDefault(scene => scene.Name == name);
            if (scene != null)
            {
                m_scenes.Remove(scene);
                if (CurrentScene == scene)
                {
                    CurrentScene = null;
                }
                SceneRemoved(scene);
            }
        }
        public void FreeASceneByID(Guid sceneID)
        {
            Scene? scene = m_scenes.FirstOrDefault(scene => scene.Id == sceneID);
            if (scene is not null)
            {
                m_scenes.Remove(scene);
                if (CurrentScene == scene)
                {
                    CurrentScene = null;
                }
                scene.Dispose();
                SceneRemoved(scene);
            }
        }

        private readonly List<Scene> m_scenes;
    }
}
