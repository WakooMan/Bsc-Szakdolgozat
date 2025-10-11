namespace SevenWonders.GameEngine
{
    public interface ISceneFileHandler
    {
        string TempPath { get; }
        string ScenesPath { get; }
        List<Scene> LoadScenes();
        void SaveScene(Scene? scene, bool checkForSceneFolder = true);

    }
}
