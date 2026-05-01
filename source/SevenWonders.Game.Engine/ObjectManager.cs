using SevenWonders.Common;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace SevenWonders.Game.Engine
{
    public class ObjectManager : IObjectManager
    {
        public ObjectManager(IInputManager inputManager, ISceneLoader sceneFileHandler, ISceneManager sceneManager)
        {
            m_subscribedInteractiveObjects = new Dictionary<IInteractiveObject, InteractiveObjectEvents>();
            m_inputManager = inputManager;
            m_sceneFileHandler = sceneFileHandler;
            m_sceneManager = sceneManager;
        }

        public void AddInteractiveObject(Scene scene, GraphicsLayer graphicsLayer, IInteractiveObject interactiveObject)
        {
            if (!scene.Layers.Contains(graphicsLayer))
            {
                throw new InvalidOperationException("The given scene does not contain the given graphics layer!");
            }

            if (interactiveObject is SceneObject sceneObject)
            {
                GameLog.Info($"Subscribing interactive object: \"{sceneObject.GetType().Name} - {sceneObject.Id} - {sceneObject.Name}\" to touch events...");
                AddSceneObject(scene, graphicsLayer, sceneObject);
                SubscribeInteractiveObjectToTouchEvents(interactiveObject, graphicsLayer);
            }
        }

        public void RemoveInteractiveObject(GraphicsLayer graphicsLayer, IInteractiveObject interactiveObject)
        {
            if (interactiveObject is SceneObject sceneObject && graphicsLayer.InteractiveObjects.Contains(interactiveObject))
            {
                GameLog.Info($"Unsubscribing interactive object: \"{sceneObject.GetType().Name} - {sceneObject.Id} - {sceneObject.Name}\" from touch events...");
                UnsubscribeInteractiveObjectToTouchEvents(interactiveObject);
                RemoveSceneObject(graphicsLayer, sceneObject);
            }
        }

        public void AddSceneObject(Scene scene, GraphicsLayer graphicsLayer, SceneObject sceneObject)
        {
            if (!scene.Layers.Contains(graphicsLayer))
            {
                throw new InvalidOperationException("The given scene does not contain the given graphics layer!");
            }

            GameLog.Info($"Adding \"{sceneObject.GetType().Name} - {sceneObject.Id} - {sceneObject.Name}\" to layer \"{graphicsLayer.Id} - {graphicsLayer.Name}\"");
            sceneObject.Id = GetNextUniqueId(scene);
            graphicsLayer.AddSceneObject(sceneObject);
            GameLog.Info($"Added \"{sceneObject.GetType().Name} - {sceneObject.Id} - {sceneObject.Name}\" to layer \"{graphicsLayer.Id} - {graphicsLayer.Name}\"");
        }

        public void AddSceneObject(GraphicsLayer graphicsLayer, SceneObject sceneObject)
        {
            if (m_sceneManager.CurrentScene is not null)
            {
                if (!m_sceneManager.CurrentScene.Layers.Contains(graphicsLayer))
                {
                    throw new InvalidOperationException("The given scene does not contain the given graphics layer!");
                }

                GameLog.Info($"Adding \"{sceneObject.GetType().Name} - {sceneObject.Id} - {sceneObject.Name}\" to layer \"{graphicsLayer.Id} - {graphicsLayer.Name}\"");
                sceneObject.Id = GetNextUniqueId(m_sceneManager.CurrentScene);
                graphicsLayer.AddSceneObject(sceneObject);
                GameLog.Info($"Added \"{sceneObject.GetType().Name} - {sceneObject.Id} - {sceneObject.Name}\" to layer \"{graphicsLayer.Id} - {graphicsLayer.Name}\"");
            }
            else
            {

                throw new InvalidOperationException("There is no scene, that is active. Cannot copy the game object!");
            }
        }

        public void RemoveSceneObject(GraphicsLayer graphicsLayer, SceneObject sceneObject)
        {
            if (graphicsLayer.SceneObjects.Contains(sceneObject))
            {
                GameLog.Info($"Removing \"{sceneObject.GetType().Name} - {sceneObject.Id} - {sceneObject.Name}\" from layer \"{graphicsLayer.Id} - {graphicsLayer.Name}\"");
                graphicsLayer.RemoveSceneObject(sceneObject);
                GameLog.Info($"Removed \"{sceneObject.GetType().Name} - {sceneObject.Id} - {sceneObject.Name}\" from layer \"{graphicsLayer.Id} - {graphicsLayer.Name}\"");
            }
        }

        public void AddGraphicsLayer(Scene scene, GraphicsLayer graphicsLayer)
        {
            GameLog.Info($"Adding GraphicsLayer \"{graphicsLayer.Id} - {graphicsLayer.Name}\" to scene \"{scene.Id} - {scene.Name}\"");
            graphicsLayer.Id = GetNextUniqueId(scene);
            scene.AddLayer(graphicsLayer);
            GameLog.Info($"Added GraphicsLayer \"{graphicsLayer.Id} - {graphicsLayer.Name}\" to scene \"{scene.Id} - {scene.Name}\"");
        }

        public void AddTexture(Scene scene, Texture texture)
        {
            if (scene.Textures.Any(t => t.FileName == texture.FileName))
            {
                throw new InvalidOperationException("The given scene contains the given texture already!");
            }

            GameLog.Info($"Adding Texture \"{texture.FileName}\" to scene \"{scene.Id} - {scene.Name}\"");
            texture.Id = GetNextUniqueId(scene);
            scene.AddTexture(texture, m_sceneFileHandler.ReceiveSceneFolder(scene));
            GameLog.Info($"Added Texture \"{texture.Id} - {texture.FileName}\" to scene \"{scene.Id} - {scene.Name}\"");
        }

        public GraphicsLayer CopyGraphicsLayer(Scene scene, GraphicsLayer graphicsLayer, string newName)
        {
            GameLog.Info($"Copying GraphicsLayer \"{graphicsLayer.Id} - {graphicsLayer.Name}\" and changing it's name to \"{newName}\"...");
            GraphicsLayer result = new GraphicsLayer(graphicsLayer);
            result.Name = newName;
            AddGraphicsLayer(scene, result);
            return result;
        }

        public GameObject CopyGameObject(Scene scene, GraphicsLayer graphicsLayer, GameObject gameObject, string newName)
        {
            GameLog.Info($"Copying GameObject \"{gameObject.Id} - {gameObject.Name}\" and changing it's name to \"{newName}\"...");
            GameObject result = new GameObject(gameObject);
            result.Name = newName;
            AddSceneObject(scene, graphicsLayer, result);
            return result;
        }

        public GameObject CopyGameObject(GraphicsLayer graphicsLayer, GameObject gameObject, string newName)
        {
            if (m_sceneManager.CurrentScene is not null)
            {
                GameLog.Info($"Copying GameObject \"{gameObject.Id} - {gameObject.Name}\" and changing it's name to \"{newName}\"...");
                GameObject result = new GameObject(gameObject);
                result.Name = newName;
                AddSceneObject(m_sceneManager.CurrentScene, graphicsLayer, result);
                return result;
            }

            throw new InvalidOperationException("There is no scene, that is active. Cannot copy the game object!");
        }

        public TextureObject CopyTextureObject(Scene scene, GraphicsLayer graphicsLayer, TextureObject textureObject, string newName)
        {
            GameLog.Info($"Copying TextureObject \"{textureObject.Id} - {textureObject.Name}\" and changing it's name to \"{newName}\"...");
            TextureObject result = new TextureObject(textureObject);
            result.Name = newName;
            AddSceneObject(scene, graphicsLayer, result);
            return result;
        }

        public ButtonObject CopyButtonObject(Scene scene, GraphicsLayer graphicsLayer, ButtonObject button, string newName)
        {
            GameLog.Info($"Copying ButtonObject \"{button.Id} - {button.Name}\" and changing it's name to \"{newName}\"...");
            ButtonObject result = new ButtonObject(button);
            result.Name = newName;
            AddSceneObject(scene, graphicsLayer, result);
            return result;
        }

        public TextLabel CopyTextLabel(Scene scene, GraphicsLayer graphicsLayer, TextLabel textLabel, string newName)
        {
            GameLog.Info($"Copying TextLabel \"{textLabel.Id} - {textLabel.Name}\" and changing it's name to \"{newName}\"...");
            TextLabel result = new TextLabel(textLabel);
            result.Name = newName;
            AddSceneObject(scene, graphicsLayer, result);
            return result;
        }

        public void SubscribeInteractiveObjectToTouchEvents(IInteractiveObject interactiveObject, GraphicsLayer graphicsLayer)
        {
            if (m_subscribedInteractiveObjects.ContainsKey(interactiveObject))
            {
                GameLog.Info("The gameobject is already subscribed to touch events.");
                return;
            }

            GameLog.Info($"Subscribing touch events for gameobject with name {interactiveObject.Name} and id {interactiveObject.Id}...");
            InteractiveObjectEvents gameObjectEvents = new InteractiveObjectEvents((args) => interactiveObject.OnTouchPressed(args, graphicsLayer),
                                                                     (args) => interactiveObject.OnTouchReleased(args, graphicsLayer),
                                                                     (args) => interactiveObject.OnTouchClicked(args, graphicsLayer),
                                                                     (args) => interactiveObject.OnTouchMoved(args, graphicsLayer));
            m_inputManager.SubscribeTouchEvent(TouchEvent.Released, SKMouseButton.Left, gameObjectEvents.TouchReleased);
            m_inputManager.SubscribeTouchEvent(TouchEvent.Pressed, SKMouseButton.Left, gameObjectEvents.TouchPressed);
            m_inputManager.SubscribeTouchEvent(TouchEvent.Moved, SKMouseButton.Left, gameObjectEvents.TouchMoved);
            m_inputManager.SubscribeTouchEvent(TouchEvent.Clicked, SKMouseButton.Left, gameObjectEvents.TouchClicked);
            m_subscribedInteractiveObjects.Add(interactiveObject, gameObjectEvents);
            GameLog.Info("Done");
        }

        public void UnsubscribeInteractiveObjectToTouchEvents(IInteractiveObject interactiveObject)
        {
            if (!m_subscribedInteractiveObjects.ContainsKey(interactiveObject))
            {
                GameLog.Info("The interactiveObject is not subscribed to touch events.");
                return;
            }

            GameLog.Info($"Unsubscribing touch events for interactiveObject with name {interactiveObject.Name} and id {interactiveObject.Id}...");
            InteractiveObjectEvents gameObjectEvents = m_subscribedInteractiveObjects[interactiveObject];
            m_inputManager.UnsubscribeTouchEvent(TouchEvent.Released, SKMouseButton.Left, gameObjectEvents.TouchReleased);
            m_inputManager.UnsubscribeTouchEvent(TouchEvent.Pressed, SKMouseButton.Left, gameObjectEvents.TouchPressed);
            m_inputManager.UnsubscribeTouchEvent(TouchEvent.Moved, SKMouseButton.Left, gameObjectEvents.TouchMoved);
            m_inputManager.UnsubscribeTouchEvent(TouchEvent.Clicked, SKMouseButton.Left, gameObjectEvents.TouchClicked);
            m_subscribedInteractiveObjects.Remove(interactiveObject);
            GameLog.Info("Done");
        }

        private static int GetNextUniqueId(Scene scene)
        {
            HashSet<int> usedIds = scene.UsedIds;

            int id = 0;
            while (usedIds.Contains(id))
            {
                id++;
            }

            return id;
        }

        private readonly IInputManager m_inputManager;
        private readonly ISceneLoader m_sceneFileHandler;
        private readonly ISceneManager m_sceneManager;
        private readonly Dictionary<IInteractiveObject, InteractiveObjectEvents> m_subscribedInteractiveObjects;
    }
}
