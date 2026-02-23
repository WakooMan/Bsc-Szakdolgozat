namespace SevenWonders.GameEngine
{
    public interface IObjectManager
    {
        void AddGraphicsLayer(Scene scene, GraphicsLayer graphicsLayer);
        void AddGameObject(Scene scene, GraphicsLayer graphicsLayer, GameObject gameObject);
        void AddTexture(Scene scene, GraphicsLayer graphicsLayer, TextureObject texture);
        GraphicsLayer CopyGraphicsLayer(Scene scene, GraphicsLayer graphicsLayer, string newName);
        GameObject CopyGameObject(Scene scene, GraphicsLayer graphicsLayer, GameObject gameObject, string newName);
        void SubscribeGameObjectToTouchEvents(GameObject gameObject, GraphicsLayer graphicsLayer);
        void UnsubscribeGameObjectToTouchEvents(GameObject gameObject);

    }
}
