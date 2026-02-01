using SevenWonders.GameEngine;

namespace SevenWondersUI
{
    public class MauiZipFileReceiver : IZipFileReceiver
    {
        public async Task<ICollection<SceneFile>> ReceiveZipFiles()
        {
            List<SceneFile> result = new List<SceneFile>();
            using var stream = await FileSystem.OpenAppPackageFileAsync(SCENE_MANIFESTFILE);
            using var reader = new StreamReader(stream);
            var content = await reader.ReadToEndAsync();

            foreach (string sceneFile in content.Split([Environment.NewLine, "\n"], StringSplitOptions.RemoveEmptyEntries).Select(f => f.Trim()))
            {
                result.Add(new SceneFile(Path.GetFileNameWithoutExtension(sceneFile), await FileSystem.OpenAppPackageFileAsync(sceneFile)));
            }

            return result;
        }

        private const string SCENE_MANIFESTFILE = "sceneManifest.txt";
    }
}
