using SkiaSharp;

namespace SevenWonders.Game.Engine
{
    public interface IObjectManager
    {
        void AddInteractiveObject(Scene scene, GraphicsLayer graphicsLayer, IInteractiveObject interactiveObject);
        void RemoveInteractiveObject(GraphicsLayer graphicsLayer, IInteractiveObject interactiveObject);
        void AddSceneObject(Scene scene, GraphicsLayer graphicsLayer, SceneObject sceneObject);

        void AddSceneObject(GraphicsLayer graphicsLayer, SceneObject sceneObject);
        void RemoveSceneObject(GraphicsLayer graphicsLayer, SceneObject sceneObject);
        void AddGraphicsLayer(Scene scene, GraphicsLayer graphicsLayer);
        void AddTexture(Scene scene, Texture texture);
        GraphicsLayer CopyGraphicsLayer(Scene scene, GraphicsLayer graphicsLayer, string newName);
        GameObject CopyGameObject(Scene scene, GraphicsLayer graphicsLayer, GameObject gameObject, string newName);
        GameObject CopyGameObject(GraphicsLayer graphicsLayer, GameObject gameObject, string newName);
        ButtonObject CopyButtonObject(Scene scene, GraphicsLayer graphicsLayer, ButtonObject button, string newName);
        TextLabel CopyTextLabel(Scene scene, GraphicsLayer graphicsLayer, TextLabel textLabel, string newName);
        TextureObject CopyTextureObject(Scene scene, GraphicsLayer graphicsLayer, TextureObject textureObject, string newName);
        void SubscribeInteractiveObjectToTouchEvents(IInteractiveObject interactiveObject, GraphicsLayer graphicsLayer);
        void UnsubscribeInteractiveObjectToTouchEvents(IInteractiveObject interactiveObject);
    }
}
