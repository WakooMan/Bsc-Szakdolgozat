using SevenWonders.AI.Model.AIModelHandler;

namespace SevenWonders.AITrainerServer
{
    public class PathProvider : IPathProvider
    {
        public string GetAppDataPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
