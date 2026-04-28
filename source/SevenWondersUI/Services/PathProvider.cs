using SevenWonders.AI.Model.AIModelHandler;

namespace SevenWondersUI.Services
{
    public class PathProvider : IPathProvider
    {
        public string GetAppDataPath()
        {
            return FileSystem.AppDataDirectory;
        }
    }
}
