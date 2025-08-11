namespace SevenWonders.GameEngine
{
    public interface ISceneManager
    {
        Scene LoadSceneXML(string sceneFilename);
        void RegisterScene(Scene scene);
        void Render();
        GameObject GetObjectByName(string name);
        Scene GetScene(uint sceneID);
        Scene GetSceneByName(string name);
        void FreeObject(uint id);
        void FreeObjects();
        void Clear();
        void FreeAScene(string name);
        void FreeASceneByID(uint sceneID);
    }
}