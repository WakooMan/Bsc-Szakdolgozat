using SevenWonders.Common;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using static SevenWonders.GameEngine.ISceneManager;

namespace SevenWonders.GameEngine
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

        public void Render(SKPaintSurfaceEventArgs eventArgs)
        {
            if (CurrentScene is null)
            {
                return;
            }

            CurrentScene.Draw(eventArgs);
        }
        public GameObject? GetObjectByName(string name)
        {
            if (CurrentScene is null)
            {
                return null;
            }

            foreach (GraphicsLayer graphicsLayer in CurrentScene.Layers)
            {
                foreach (GameObject gameObject in graphicsLayer.ObjectList)
                {
                    if (gameObject.Name == name)
                    {
                        return gameObject;
                    }
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
                SceneRemoved(scene);
            }
        }
        public void FreeASceneByID(Guid sceneID)
        {
            Scene? scene = m_scenes.FirstOrDefault(scene => scene.Id == sceneID);
            if (scene != null)
            {
                m_scenes.Remove(scene);
                SceneRemoved(scene);
            }
        }

        private readonly List<Scene> m_scenes;
    }
}
