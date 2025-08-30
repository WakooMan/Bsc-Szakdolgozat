using SevenWonders.GameEngine;

namespace SevenWonders.SceneEditor.Helpers
{
    public static class CopyHelper
    {
        public static int BiggestId { get; private set; }

        public static GraphicsLayer CopyLayer(GraphicsLayer graphicsLayer, string name)
        {
            GraphicsLayer result = new GraphicsLayer(graphicsLayer);
            result.Name = new string(name);
            AddIdToGraphicsLayer(result);
            result.LoadTextures(FileHelper.TempPath);
            return result;
        }

        public static GameObject CopyGameObject(GameObject gameObject, string name)
        {
            GameObject result = new GameObject(gameObject);
            gameObject.Name = new string(name);
            AddIdToGameObject(result);
            gameObject.LoadTextures(FileHelper.TempPath);

            return result;
        }

        public static void OnSceneChanged(Scene? scene)
        {
            BiggestId = 0;
            if (scene is null)
            {
                return;
            }

            scene.Id = BiggestId;
            scene.Layers.ForEach(AddIdToGraphicsLayer);
        }

        private static void AddIdToGraphicsLayer(GraphicsLayer graphicsLayer)
        {
            graphicsLayer.ID = ++BiggestId;
            foreach (var texture in graphicsLayer.Textures)
            {
                AddIdToTexture(texture);
            }

            foreach (var gameObject in graphicsLayer.ObjectList)
            {
                AddIdToGameObject(gameObject);
            }
        }

        private static void AddIdToGameObject(GameObject gameObject)
        {
            gameObject.Id = ++BiggestId;
            gameObject.Animations.ForEach(anim => anim.Frames.ForEach(frame => AddIdToTexture(frame.Frame)));
        }

        private static void AddIdToTexture(Texture texture)
        {
            texture.Id= ++BiggestId;
        }
    }
}
