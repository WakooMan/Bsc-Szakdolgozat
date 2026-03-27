using SkiaSharp.Views.Maui;

namespace SevenWonders.GameEngine
{
    public interface ISceneManager
    {
        delegate void SceneEvent(Scene scene);

        event SceneEvent SceneRegistered;
        event SceneEvent SceneRemoved;

        Scene? CurrentScene { get; }
        IReadOnlyList<Scene> Scenes { get; }
        void SetCurrentScene(Scene scene);
        void RegisterScene(Scene scene);
        void Render(SKPaintSurfaceEventArgs eventArgs);
        GameObject? GetObjectByName(string name);
        ButtonObject? GetButtonByName(string name);
        GraphicsLayer? GetLayerByName(string name);
        IInteractiveObject? GetInteractiveObjectByName(string name);
        Scene GetScene(Guid sceneID);
        Scene GetSceneByName(string name);
        void FreeObject(int id);
        void FreeObjects();
        void Clear();
        void FreeAScene(string name);
        void FreeASceneByID(Guid sceneID);
    }
}