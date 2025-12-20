namespace SevenWonders.GameEngine
{
    public class ObjectManager : IObjectManager
    {

        public ObjectManager(IInputManager inputManager, ISceneFileHandler sceneFileHandler)
        {
            m_inputManager = inputManager;
            m_sceneFileHandler = sceneFileHandler;
        }
        public void AddGameObject(Scene scene, GraphicsLayer graphicsLayer, GameObject gameObject)
        {
            if (!scene.Layers.Contains(graphicsLayer))
            {
                throw new InvalidOperationException("The given scene does not contain the given graphics layer!");
            }

            gameObject.LoadTextures(m_sceneFileHandler.ReceiveSceneFolder(scene));
            gameObject.Id = scene.BiggestId++;
            m_inputManager.SubscribeMouseEvent(MouseEvent.MouseClicked, MouseButton.Right, gameObject.OnRightMouseClicked);
            m_inputManager.SubscribeMouseEvent(MouseEvent.MouseUp, MouseButton.Right, gameObject.OnRightMouseUp);
            m_inputManager.SubscribeMouseEvent(MouseEvent.MouseDown, MouseButton.Right, gameObject.OnRightMouseDown);
            m_inputManager.SubscribeMouseEvent(MouseEvent.MouseClicked, MouseButton.Left, gameObject.OnLeftMouseClicked);
            m_inputManager.SubscribeMouseEvent(MouseEvent.MouseUp, MouseButton.Left, gameObject.OnLeftMouseUp);
            m_inputManager.SubscribeMouseEvent(MouseEvent.MouseDown, MouseButton.Left, gameObject.OnLeftMouseDown);
            m_inputManager.SubscribeMouseEvent(MouseEvent.MouseMove, MouseButton.None, gameObject.OnMouseMove);
            graphicsLayer.ObjectList.Add(gameObject);
        }

        public void RemoveGameObject(GraphicsLayer graphicsLayer, GameObject gameObject)
        {
            if (graphicsLayer.ObjectList.Contains(gameObject))
            {
                m_inputManager.UnsubscribeMouseEvent(MouseEvent.MouseClicked, MouseButton.Right, gameObject.OnRightMouseClicked);
                m_inputManager.UnsubscribeMouseEvent(MouseEvent.MouseUp, MouseButton.Right, gameObject.OnRightMouseUp);
                m_inputManager.UnsubscribeMouseEvent(MouseEvent.MouseDown, MouseButton.Right, gameObject.OnRightMouseDown);
                m_inputManager.UnsubscribeMouseEvent(MouseEvent.MouseClicked, MouseButton.Left, gameObject.OnLeftMouseClicked);
                m_inputManager.UnsubscribeMouseEvent(MouseEvent.MouseUp, MouseButton.Left, gameObject.OnLeftMouseUp);
                m_inputManager.UnsubscribeMouseEvent(MouseEvent.MouseDown, MouseButton.Left, gameObject.OnLeftMouseDown);
                m_inputManager.UnsubscribeMouseEvent(MouseEvent.MouseMove, MouseButton.None, gameObject.OnMouseMove);
                graphicsLayer.ObjectList.Remove(gameObject);
            }
        }

        public void AddGraphicsLayer(Scene scene, GraphicsLayer graphicsLayer)
        {
            graphicsLayer.LoadTextures(m_sceneFileHandler.ReceiveSceneFolder(scene));
            graphicsLayer.ID = scene.BiggestId++;
            scene.Layers.Add(graphicsLayer);
        }

        public void AddTexture(Scene scene, GraphicsLayer graphicsLayer, TextureObject texture)
        {
            if (!scene.Layers.Contains(graphicsLayer))
            {
                throw new InvalidOperationException("The given scene does not contain the given graphics layer!");
            }

            texture.LoadTexture(m_sceneFileHandler.ReceiveSceneFolder(scene));
            texture.Id = scene.BiggestId++;
            graphicsLayer.Textures.Add(texture);
        }

        public GraphicsLayer CopyGraphicsLayer(Scene scene, GraphicsLayer graphicsLayer, string newName)
        {
            GraphicsLayer result = new GraphicsLayer(graphicsLayer);
            result.Name = newName;
            AddGraphicsLayer(scene, result);
            return result;
        }

        public GameObject CopyGameObject(Scene scene, GraphicsLayer graphicsLayer, GameObject gameObject, string newName)
        {
            GameObject result = new GameObject(gameObject);
            result.Name = newName;
            AddGameObject(scene, graphicsLayer, result);
            return result;
        }

        private readonly IInputManager m_inputManager;
        private readonly ISceneFileHandler m_sceneFileHandler;
    }
}
