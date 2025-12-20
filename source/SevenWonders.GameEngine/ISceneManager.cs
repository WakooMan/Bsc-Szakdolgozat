using SkiaSharp.Views.Maui;

namespace SevenWonders.GameEngine
{
    public interface ISceneManager
    {
        Scene? CurrentScene { get; }
        IReadOnlyList<Scene> Scenes { get; }
        void SetCurrentScene(Scene scene);
        void RegisterScene(Scene scene);
        void Render(SKPaintSurfaceEventArgs eventArgs);
        GameObject GetObjectByName(string name);
        Scene GetScene(Guid sceneID);
        Scene GetSceneByName(string name);
        void FreeObject(uint id);
        void FreeObjects();
        void Clear();
        void FreeAScene(string name);
        void FreeASceneByID(Guid sceneID);
    }
}