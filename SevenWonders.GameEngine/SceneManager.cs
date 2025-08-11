using SkiaSharp;
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
        public List<Scene> Scenes;
        public bool DrawBoundingBox;
        public SKColor BoundingShapeColor;

        public SceneManager()
        {
            Scenes = new List<Scene>();
        }

        public Scene LoadSceneXML(string sceneFilename)
        {
            return new Scene();
        }
        public void RegisterScene(Scene scene)
        {
            if (!Scenes.Contains(scene))
            {
                Scenes.Add(scene);
            }
        }
        public void Render()
        {

        }
        public GameObject GetObjectByName(string name)
        {
            return new GameObject();
        }
        public Scene GetScene(uint sceneID)
        {
            return Scenes.First(scene => scene.Id == sceneID);
        }
        public Scene GetSceneByName(string name)
        {
            return Scenes.First(scene => scene.Name == name);
        }
        public void FreeObject(uint id)
        {

        }
        public void FreeObjects()
        {

        }
        public void Clear()
        {
            Scenes.Clear();
        }
        public void FreeAScene(string name)
        {
            Scenes.Remove(Scenes.First(scene => scene.Name == name));
        }
        public void FreeASceneByID(uint sceneID)
        {
            Scenes.Remove(Scenes.First(scene => scene.Id == sceneID));
        }
    }
}
