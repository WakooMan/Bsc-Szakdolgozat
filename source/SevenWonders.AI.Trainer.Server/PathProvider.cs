using SevenWonders.Common;

namespace SevenWonders.AI.Trainer.Server
{
    public class PathProvider : IPathProvider
    {
        public string GetAppDataPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
