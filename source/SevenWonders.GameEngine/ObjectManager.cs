using SevenWonders.Common;
using SkiaSharp.Views.Maui;

namespace SevenWonders.GameEngine
{
    public class ObjectManager : IObjectManager
    {
        public ObjectManager(IInputManager inputManager, ISceneLoader sceneFileHandler)
        {
            m_subscribedGameObjects = new Dictionary<GameObject, GameObjectEvents>();
            m_inputManager = inputManager;
            m_sceneFileHandler = sceneFileHandler;
        }
        public void AddGameObject(Scene scene, GraphicsLayer graphicsLayer, GameObject gameObject)
        {
            if (!scene.Layers.Contains(graphicsLayer))
            {
                throw new InvalidOperationException("The given scene does not contain the given graphics layer!");
            }

            GameLog.Info($"Adding GameObject \"{gameObject.Id} - {gameObject.Name}\" to layer \"{graphicsLayer.ID} - {graphicsLayer.Name}\"");
            GameLog.Info("Loading textures...");
            gameObject.LoadTextures(m_sceneFileHandler.ReceiveSceneFolder(scene));
            GameLog.Info("Done");
            gameObject.Id = scene.BiggestId++;
            SubscribeGameObjectToTouchEvents(gameObject, graphicsLayer);
            graphicsLayer.ObjectList.Add(gameObject);
            GameLog.Info($"Added GameObject \"{gameObject.Id} - {gameObject.Name}\" to layer \"{graphicsLayer.ID} - {graphicsLayer.Name}\"");
        }

        public void RemoveGameObject(GraphicsLayer graphicsLayer, GameObject gameObject)
        {
            if (graphicsLayer.ObjectList.Contains(gameObject))
            {
                GameLog.Info($"Removing GameObject \"{gameObject.Id} - {gameObject.Name}\" from layer \"{graphicsLayer.ID} - {graphicsLayer.Name}\"");
                UnsubscribeGameObjectToTouchEvents(gameObject);
                graphicsLayer.ObjectList.Remove(gameObject);
                GameLog.Info($"Removed GameObject \"{gameObject.Id} - {gameObject.Name}\" from layer \"{graphicsLayer.ID} - {graphicsLayer.Name}\"");
            }
        }

        public void AddGraphicsLayer(Scene scene, GraphicsLayer graphicsLayer)
        {
            GameLog.Info($"Adding GraphicsLayer \"{graphicsLayer.ID} - {graphicsLayer.Name}\" to scene \"{scene.Id} - {scene.Name}\"");
            GameLog.Info("Loading textures...");
            graphicsLayer.LoadTextures(m_sceneFileHandler.ReceiveSceneFolder(scene));
            GameLog.Info("Done");
            graphicsLayer.ID = scene.BiggestId++;
            scene.Layers.Add(graphicsLayer);
            GameLog.Info($"Added GraphicsLayer \"{graphicsLayer.ID} - {graphicsLayer.Name}\" to scene \"{scene.Id} - {scene.Name}\"");
        }

        public void AddTexture(Scene scene, GraphicsLayer graphicsLayer, TextureObject texture)
        {
            if (!scene.Layers.Contains(graphicsLayer))
            {
                throw new InvalidOperationException("The given scene does not contain the given graphics layer!");
            }

            GameLog.Info($"Adding Texture \"{texture.Id} - {texture.Name}\" to layer \"{graphicsLayer.ID} - {graphicsLayer.Name}\"");
            GameLog.Info("Loading texture...");
            texture.LoadTexture(m_sceneFileHandler.ReceiveSceneFolder(scene));
            GameLog.Info("Done");
            texture.Id = scene.BiggestId++;
            graphicsLayer.Textures.Add(texture);
            GameLog.Info($"Added Texture \"{texture.Id} - {texture.Name}\" to layer \"{graphicsLayer.ID} - {graphicsLayer.Name}\"");
        }

        public GraphicsLayer CopyGraphicsLayer(Scene scene, GraphicsLayer graphicsLayer, string newName)
        {
            GameLog.Info($"Copying GraphicsLayer \"{graphicsLayer.ID} - {graphicsLayer.Name}\" and changing it's name to \"{newName}\"...");
            GraphicsLayer result = new GraphicsLayer(graphicsLayer);
            result.Name = newName;
            GameLog.Info("Done");
            AddGraphicsLayer(scene, result);
            return result;
        }

        public GameObject CopyGameObject(Scene scene, GraphicsLayer graphicsLayer, GameObject gameObject, string newName)
        {
            GameLog.Info($"Copying GameObject \"{gameObject.Id} - {gameObject.Name}\" and changing it's name to \"{newName}\"...");
            GameObject result = new GameObject(gameObject);
            result.Name = newName;
            GameLog.Info("Done");
            AddGameObject(scene, graphicsLayer, result);
            return result;
        }

        public void SubscribeGameObjectToTouchEvents(GameObject gameObject, GraphicsLayer graphicsLayer)
        {
            if (m_subscribedGameObjects.ContainsKey(gameObject))
            {
                GameLog.Info("The gameobject is already subscribed to touch events.");
                return;
            }

            GameLog.Info($"Subscribing touch events for gameobject with name {gameObject.Name} and id {gameObject.Id}...");
            GameObjectEvents gameObjectEvents = new GameObjectEvents((args) => gameObject.OnTouchPressed(args, graphicsLayer),
                                                                     (args) => gameObject.OnTouchReleased(args, graphicsLayer),
                                                                     (args) => gameObject.OnTouchClicked(args, graphicsLayer),
                                                                     (args) => gameObject.OnTouchMoved(args, graphicsLayer));
            m_inputManager.SubscribeTouchEvent(TouchEvent.Released, SKMouseButton.Left, gameObjectEvents.TouchReleased);
            m_inputManager.SubscribeTouchEvent(TouchEvent.Pressed, SKMouseButton.Left, gameObjectEvents.TouchPressed);
            m_inputManager.SubscribeTouchEvent(TouchEvent.Moved, SKMouseButton.Left, gameObjectEvents.TouchMoved);
            m_inputManager.SubscribeTouchEvent(TouchEvent.Clicked, SKMouseButton.Left, gameObjectEvents.TouchClicked);
            m_subscribedGameObjects.Add(gameObject, gameObjectEvents);
            GameLog.Info("Done");
        }

        public void UnsubscribeGameObjectToTouchEvents(GameObject gameObject)
        {
            if (!m_subscribedGameObjects.ContainsKey(gameObject))
            {
                GameLog.Info("The gameobject is not subscribed to touch events.");
                return;
            }

            GameLog.Info($"Unsubscribing touch events for gameobject with name {gameObject.Name} and id {gameObject.Id}...");
            GameObjectEvents gameObjectEvents = m_subscribedGameObjects[gameObject];
            m_inputManager.UnsubscribeTouchEvent(TouchEvent.Released, SKMouseButton.Left, gameObjectEvents.TouchReleased);
            m_inputManager.UnsubscribeTouchEvent(TouchEvent.Pressed, SKMouseButton.Left, gameObjectEvents.TouchPressed);
            m_inputManager.UnsubscribeTouchEvent(TouchEvent.Moved, SKMouseButton.Left, gameObjectEvents.TouchMoved);
            m_inputManager.UnsubscribeTouchEvent(TouchEvent.Clicked, SKMouseButton.Left, gameObjectEvents.TouchClicked);
            m_subscribedGameObjects.Remove(gameObject);
            GameLog.Info("Done");
        }

        private readonly IInputManager m_inputManager;
        private readonly ISceneLoader m_sceneFileHandler;
        private readonly Dictionary<GameObject, GameObjectEvents> m_subscribedGameObjects;
    }
}
