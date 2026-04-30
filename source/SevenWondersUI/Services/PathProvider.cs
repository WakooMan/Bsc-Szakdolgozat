using SevenWonders.Common;

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
