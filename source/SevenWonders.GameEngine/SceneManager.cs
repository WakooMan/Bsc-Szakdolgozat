using SevenWonders.Common;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.GameEngine
{
    public class SceneManager: ISceneManager
    {
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
        public GameObject GetObjectByName(string name)
        {
            return new GameObject();
        }
        public Scene GetScene(Guid sceneID)
        {
            return m_scenes.First(scene => scene.Id == sceneID);
        }
        public Scene GetSceneByName(string name)
        {
            return m_scenes.First(scene => scene.Name == name);
        }
        public void FreeObject(uint id)
        {

        }
        public void FreeObjects()
        {

        }
        public void Clear()
        {
            m_scenes.Clear();
        }
        public void FreeAScene(string name)
        {
            m_scenes.Remove(m_scenes.First(scene => scene.Name == name));
        }
        public void FreeASceneByID(Guid sceneID)
        {
            m_scenes.Remove(m_scenes.First(scene => scene.Id == sceneID));
        }

        private readonly List<Scene> m_scenes;
    }
}
