using SevenWonders.Common;

namespace SevenWonders.SceneEditor
{
    public class PathProvider: IPathProvider
    {
        public string GetAppDataPath()
        {
            return FileSystem.AppDataDirectory;
        }
    }
}
