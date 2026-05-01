using SevenWonders.Common;

namespace SevenWonders.UI.Services
{
    public class PathProvider : IPathProvider
    {
        public string GetAppDataPath()
        {
            return FileSystem.AppDataDirectory;
        }
    }
}
