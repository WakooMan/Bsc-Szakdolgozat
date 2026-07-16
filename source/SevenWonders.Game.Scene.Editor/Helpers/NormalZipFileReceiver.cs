using SevenWonders.Game.Engine;
using SevenWonders.Game.Engine.SceneHandling;

namespace SevenWonders.Game.Scene.Editor.Helpers
{
    public class NormalZipFileReceiver : IZipFileReceiver
    {
        public string ScenesPath { get; }

        public NormalZipFileReceiver()
        {
            ScenesPath = Path.Combine(Directory.GetCurrentDirectory(), "Scenes");
        }

        public Task<ICollection<SceneFile>> ReceiveZipFiles()
        {
            ICollection<SceneFile> collection = Directory.GetFiles(ScenesPath).Where(file => file.EndsWith(".zip")).Select(sceneFile => new SceneFile(Path.GetFileNameWithoutExtension(sceneFile), File.OpenRead(sceneFile))).ToList();
            return Task.FromResult(collection);
        }
    }
}
