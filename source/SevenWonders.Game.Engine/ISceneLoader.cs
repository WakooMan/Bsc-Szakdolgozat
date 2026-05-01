namespace SevenWonders.Game.Engine
{
    public interface ISceneLoader
    {
        Task<ICollection<Scene>> LoadScenes();
        void SaveScene(Scene? scene, bool checkForSceneFolder = true);
        string ReceiveSceneFolder(Scene scene);

    }
}
