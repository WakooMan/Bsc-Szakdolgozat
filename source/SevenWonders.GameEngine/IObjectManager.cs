namespace SevenWonders.GameEngine
{
    public interface IObjectManager
    {
        void AddGraphicsLayer(Scene scene, GraphicsLayer graphicsLayer);
        void AddGameObject(Scene scene, GraphicsLayer graphicsLayer, GameObject gameObject);
        void RemoveGameObject(GraphicsLayer graphicsLayer, GameObject gameObject);
        void AddTextureObject(Scene scene, GraphicsLayer graphicsLayer, TextureObject texture);
        void AddButtonObject(Scene scene, GraphicsLayer graphicsLayer, ButtonObject button);
        void AddTextLabel(Scene scene, GraphicsLayer graphicsLayer, TextLabel textLabel);
        void AddTexture(Scene scene, Texture texture);
        GraphicsLayer CopyGraphicsLayer(Scene scene, GraphicsLayer graphicsLayer, string newName);
        GameObject CopyGameObject(Scene scene, GraphicsLayer graphicsLayer, GameObject gameObject, string newName);
        ButtonObject CopyButtonObject(Scene scene, GraphicsLayer graphicsLayer, ButtonObject button, string newName);
        TextLabel CopyTextLabel(Scene scene, GraphicsLayer graphicsLayer, TextLabel textLabel, string newName);
        TextureObject CopyTextureObject(Scene scene, GraphicsLayer graphicsLayer, TextureObject textureObject, string newName);
        void SubscribeGameObjectToTouchEvents(GameObject gameObject, GraphicsLayer graphicsLayer);
        void UnsubscribeGameObjectToTouchEvents(GameObject gameObject);
        void SubscribeButtonToTouchEvents(ButtonObject button, GraphicsLayer graphicsLayer);
        void UnsubscribeButtonToTouchEvents(ButtonObject button);
    }
}
