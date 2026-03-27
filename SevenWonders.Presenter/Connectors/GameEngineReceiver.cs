using SevenWonders.GameEngine;

namespace SevenWonders.Presenter.Connectors
{
    public class GameEngineReceiver : IGameEngineReceiver
    {
        public GameEngineReceiver(ISceneManager sceneManager)
        {
            m_sceneManager = sceneManager;
        }

        public GameObject ReceiveGameObject(string name)
        {
            GameObject? gameObject = m_sceneManager.GetObjectByName(name);
            if (gameObject is null)
            {
                throw new InvalidOperationException($"GameObject with name {name} does not exist.");
            }
            return gameObject;
        }

        public IInteractiveObject ReceiveInteractiveObject(string name)
        {
            IInteractiveObject? interactiveObject = m_sceneManager.GetInteractiveObjectByName(name);
            if (interactiveObject is null)
            {
                throw new InvalidOperationException($"GameObject with name {name} does not exist.");
            }
            return interactiveObject;
        }

        public ICollection<GameObject> ReceiveGameObjects(string name, int number)
        {
            List<GameObject> result = new List<GameObject>();
            for (int i = 1; i <= number; i++)
            {
                string targetName = $"{name}{i}";
                GameObject? gameObject = m_sceneManager.GetObjectByName(targetName);
                if (gameObject is null)
                {
                    throw new InvalidOperationException($"GameObject with name {targetName}, does not exist.");
                }
                result.Add(gameObject);
            }

            return result;
        }

        public GraphicsLayer ReceiveGraphicsLayer(string name)
        {
            GraphicsLayer? layer = m_sceneManager.GetLayerByName(name);
            if (layer is null)
            {
                throw new InvalidOperationException($"GraphicsLayer with name {name} does not exist.");
            }
            return layer;
        }

        public ButtonObject ReceiveButton(string name)
        {
            ButtonObject? button = m_sceneManager.GetButtonByName(name);
            if (button is null)
            {
                throw new InvalidOperationException($"ButtonObject with name {name} does not exist.");
            }
            return button;
        }

        private readonly ISceneManager m_sceneManager;
    }
}
