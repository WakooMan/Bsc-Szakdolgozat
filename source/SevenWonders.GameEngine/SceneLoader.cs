using SevenWonders.Common;
using System.IO.Compression;

namespace SevenWonders.GameEngine
{
    public class SceneLoader : ISceneLoader
    {
        public virtual string ScenesPath => Path.Combine(Directory.GetCurrentDirectory(), "Scenes");

        public SceneLoader(IXmlHandler xmlHandler, IZipFileReceiver zipFileReceiver)
        {
            ArgumentChecker.CheckNull(xmlHandler, nameof(xmlHandler));

            m_tempPath = Path.Combine(Directory.GetCurrentDirectory(), "ScenesTemp");
            m_xmlHandler = xmlHandler;
            m_zipFileReceiver = zipFileReceiver;
        }

        public async Task<ICollection<Scene>> LoadScenes()
        {
            List<Scene> result = new List<Scene>();
            if (!Directory.Exists(m_tempPath))
            {
                GameLog.Info($"Creating directory: \"{m_tempPath}\"");
                Directory.CreateDirectory(m_tempPath);
            }
            // Directory.GetFiles(ScenesPath).Where(file => file.EndsWith(".zip"))
            foreach (SceneFile sceneFile in await m_zipFileReceiver.ReceiveZipFiles())
            {
                result.Add(LoadScene(sceneFile));
            }

            return result;
        }

        private Scene LoadScene(SceneFile sceneFile)
        {
            GameLog.Info($"Loading scene from file: \"{sceneFile.Name}\"");
            string extractedSceneLocation = Path.Combine(m_tempPath, sceneFile.Name);

            if (Directory.Exists(extractedSceneLocation))
            {
                GameLog.Info($"Deleting directory: \"{extractedSceneLocation}\"");
                Directory.Delete(extractedSceneLocation, true);
            }

            ZipFile.ExtractToDirectory(sceneFile.Stream, extractedSceneLocation);
            Scene? scene = m_xmlHandler.Deserialize<Scene>(Path.Combine(extractedSceneLocation, "scene.xml"));
            ArgumentChecker.CheckPredicateForOperation(() => scene is null, $"The scene xml file could not be loaded correctly! Check the format of scene.xml in \"{sceneFile.Name}\" zip file.");
            scene.LoadTextures(extractedSceneLocation);
            GameLog.Info($"Scene loaded: \"{scene.Id} - {scene.Name}\"");
            return scene;
        }

        public void SaveScene(Scene? scene, bool checkForSceneFolder = true)
        {
            ArgumentChecker.CheckNull(scene, nameof(scene));

            GameLog.Info($"Saving scene: \"{scene.Id} - {scene.Name}\"");
            string savingScenePath = Path.Combine(m_tempPath, scene?.Name ?? string.Empty);
            ArgumentChecker.CheckPredicateForOperation(() => !Directory.Exists(savingScenePath) && checkForSceneFolder, $"Cannot save scene, because the \"{scene.Name}\" folder does not exist");

            if (!Directory.Exists(savingScenePath))
            {
                GameLog.Info($"Creating directory: \"{savingScenePath}\"");
                Directory.CreateDirectory(savingScenePath);
                GameLog.Info("Done");
            }

            if (!Directory.Exists(ScenesPath))
            {
                GameLog.Info($"Creating directory: \"{ScenesPath}\"");
                Directory.CreateDirectory(ScenesPath);
                GameLog.Info("Done");
            }

            string sceneXmlPath = Path.Combine(savingScenePath, "scene.xml");
            GameLog.Info("Saving scene.xml file...");
            m_xmlHandler.Serialize(sceneXmlPath, scene);
            GameLog.Info("Done");

            string zipPath = Path.Combine(ScenesPath, $"{scene.Name}.zip");

            if (File.Exists(zipPath))
            {
                GameLog.Info($"Deleting old zip file: \"{zipPath}\"");
                File.Delete(zipPath);
            }

            ZipFile.CreateFromDirectory(
               savingScenePath,
               zipPath,
               CompressionLevel.Optimal,
               includeBaseDirectory: false);
            GameLog.Info($"Saved scene: \"{scene.Id} - {scene.Name}\" to file \"{zipPath}\"");
        }

        public string ReceiveSceneFolder(Scene scene)
        {
            return Path.Combine(m_tempPath, scene.Name);
        }

        private readonly IXmlHandler m_xmlHandler;
        private readonly IZipFileReceiver m_zipFileReceiver;
        private readonly string m_tempPath;
    }
}
