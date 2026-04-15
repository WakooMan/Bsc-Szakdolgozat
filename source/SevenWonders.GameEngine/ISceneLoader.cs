using SkiaSharp;

namespace SevenWonders.GameEngine
{
    public interface ISceneLoader
    {
        Task<ICollection<Scene>> LoadScenes(GRContext gRContext);
        void SaveScene(Scene? scene, bool checkForSceneFolder = true);
        string ReceiveSceneFolder(Scene scene);

    }
}
