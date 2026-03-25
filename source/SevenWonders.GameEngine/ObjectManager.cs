using SevenWonders.Common;
using SkiaSharp.Views.Maui;

namespace SevenWonders.GameEngine
{
    public class ObjectManager : IObjectManager
    {
        public ObjectManager(IInputManager inputManager, ISceneLoader sceneFileHandler)
        {
            m_subscribedGameObjects = new Dictionary<GameObject, GameObjectEvents>();
            m_subscribedButtons = new Dictionary<ButtonObject, GameObjectEvents>();
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
            GameLog.Info("Done");
            graphicsLayer.ID = scene.BiggestId++;
            scene.Layers.Add(graphicsLayer);
            GameLog.Info($"Added GraphicsLayer \"{graphicsLayer.ID} - {graphicsLayer.Name}\" to scene \"{scene.Id} - {scene.Name}\"");
        }

        public void AddTextureObject(Scene scene, GraphicsLayer graphicsLayer, TextureObject texture)
        {
            if (!scene.Layers.Contains(graphicsLayer))
            {
                throw new InvalidOperationException("The given scene does not contain the given graphics layer!");
            }

            GameLog.Info($"Adding Texture \"{texture.Id} - {texture.Name}\" to layer \"{graphicsLayer.ID} - {graphicsLayer.Name}\"");
            GameLog.Info("Done");
            texture.Id = scene.BiggestId++;
            graphicsLayer.TextureObjects.Add(texture);
            GameLog.Info($"Added Texture \"{texture.Id} - {texture.Name}\" to layer \"{graphicsLayer.ID} - {graphicsLayer.Name}\"");
        }

        public void AddButtonObject(Scene scene, GraphicsLayer graphicsLayer, ButtonObject button)
        {
            if (!scene.Layers.Contains(graphicsLayer))
            {
                throw new InvalidOperationException("The given scene does not contain the given graphics layer!");
            }

            GameLog.Info($"Adding ButtonObject \"{button.Id} - {button.Name}\" to layer \"{graphicsLayer.ID} - {graphicsLayer.Name}\"");
            GameLog.Info("Done");
            button.Id = scene.BiggestId++;
            SubscribeButtonToTouchEvents(button, graphicsLayer);
            graphicsLayer.Buttons.Add(button);
            GameLog.Info($"Added ButtonObject \"{button.Id} - {button.Name}\" to layer \"{graphicsLayer.ID} - {graphicsLayer.Name}\"");
        }

        public void AddTextLabel(Scene scene, GraphicsLayer graphicsLayer, TextLabel textLabel)
        {
            if (!scene.Layers.Contains(graphicsLayer))
            {
                throw new InvalidOperationException("The given scene does not contain the given graphics layer!");
            }

            GameLog.Info($"Adding TextLabel \"{textLabel.Id} - {textLabel.Name}\" to layer \"{graphicsLayer.ID} - {graphicsLayer.Name}\"");
            GameLog.Info("Done");
            textLabel.Id = scene.BiggestId++;
            graphicsLayer.TextLabels.Add(textLabel);
            GameLog.Info($"Added TextLabel \"{textLabel.Id} - {textLabel.Name}\" to layer \"{graphicsLayer.ID} - {graphicsLayer.Name}\"");
        }

        public void AddTexture(Scene scene, Texture texture)
        {
            if (scene.Textures.Any(t => t.FileName == texture.FileName))
            {
                throw new InvalidOperationException("The given scene contains the given texture already!");
            }

            GameLog.Info($"Adding Texture \"{texture.FileName}\" to scene \"{scene.Id} - {scene.Name}\"");
            GameLog.Info("Done");
            texture.Id = scene.BiggestId++;
            scene.AddTexture(texture, m_sceneFileHandler.ReceiveSceneFolder(scene));
            GameLog.Info($"Added Texture \"{texture.Id} - {texture.FileName}\" to scene \"{scene.Id} - {scene.Name}\"");
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

        public TextureObject CopyTextureObject(Scene scene, GraphicsLayer graphicsLayer, TextureObject textureObject, string newName)
        {
            GameLog.Info($"Copying TextureObject \"{textureObject.Id} - {textureObject.Name}\" and changing it's name to \"{newName}\"...");
            TextureObject result = new TextureObject(textureObject);
            result.Name = newName;
            GameLog.Info("Done");
            AddTextureObject(scene, graphicsLayer, result);
            return result;
        }

        public ButtonObject CopyButtonObject(Scene scene, GraphicsLayer graphicsLayer, ButtonObject button, string newName)
        {
            GameLog.Info($"Copying ButtonObject \"{button.Id} - {button.Name}\" and changing it's name to \"{newName}\"...");
            ButtonObject result = new ButtonObject(button);
            result.Name = newName;
            GameLog.Info("Done");
            AddButtonObject(scene, graphicsLayer, result);
            return result;
        }

        public TextLabel CopyTextLabel(Scene scene, GraphicsLayer graphicsLayer, TextLabel textLabel, string newName)
        {
            GameLog.Info($"Copying TextLabel \"{textLabel.Id} - {textLabel.Name}\" and changing it's name to \"{newName}\"...");
            TextLabel result = new TextLabel(textLabel);
            result.Name = newName;
            GameLog.Info("Done");
            AddTextLabel(scene, graphicsLayer, result);
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

        public void SubscribeButtonToTouchEvents(ButtonObject button, GraphicsLayer graphicsLayer)
        {
            if (m_subscribedButtons.ContainsKey(button))
            {
                GameLog.Info("The button is already subscribed to touch events.");
                return;
            }

            GameLog.Info($"Subscribing touch events for button with name {button.Name} and id {button.Id}...");
            GameObjectEvents buttonEvents = new GameObjectEvents((args) => button.OnTouchPressed(args, graphicsLayer),
                                                                  (args) => button.OnTouchReleased(args, graphicsLayer),
                                                                  (args) => button.OnTouchClicked(args, graphicsLayer),
                                                                  (args) => button.OnTouchMoved(args, graphicsLayer));
            m_inputManager.SubscribeTouchEvent(TouchEvent.Released, SKMouseButton.Left, buttonEvents.TouchReleased);
            m_inputManager.SubscribeTouchEvent(TouchEvent.Pressed, SKMouseButton.Left, buttonEvents.TouchPressed);
            m_inputManager.SubscribeTouchEvent(TouchEvent.Moved, SKMouseButton.Left, buttonEvents.TouchMoved);
            m_inputManager.SubscribeTouchEvent(TouchEvent.Clicked, SKMouseButton.Left, buttonEvents.TouchClicked);
            m_subscribedButtons.Add(button, buttonEvents);
            GameLog.Info("Done");
        }

        public void UnsubscribeButtonToTouchEvents(ButtonObject button)
        {
            if (!m_subscribedButtons.ContainsKey(button))
            {
                GameLog.Info("The button is not subscribed to touch events.");
                return;
            }

            GameLog.Info($"Unsubscribing touch events for button with name {button.Name} and id {button.Id}...");
            GameObjectEvents buttonEvents = m_subscribedButtons[button];
            m_inputManager.UnsubscribeTouchEvent(TouchEvent.Released, SKMouseButton.Left, buttonEvents.TouchReleased);
            m_inputManager.UnsubscribeTouchEvent(TouchEvent.Pressed, SKMouseButton.Left, buttonEvents.TouchPressed);
            m_inputManager.UnsubscribeTouchEvent(TouchEvent.Moved, SKMouseButton.Left, buttonEvents.TouchMoved);
            m_inputManager.UnsubscribeTouchEvent(TouchEvent.Clicked, SKMouseButton.Left, buttonEvents.TouchClicked);
            m_subscribedButtons.Remove(button);
            GameLog.Info("Done");
        }

        private readonly IInputManager m_inputManager;
        private readonly ISceneLoader m_sceneFileHandler;
        private readonly Dictionary<GameObject, GameObjectEvents> m_subscribedGameObjects;
        private readonly Dictionary<ButtonObject, GameObjectEvents> m_subscribedButtons;
    }
}
